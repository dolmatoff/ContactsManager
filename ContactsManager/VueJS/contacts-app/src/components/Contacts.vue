<template>
 <v-card>
    <v-card-title>
      Contacts
      <v-spacer></v-spacer>
      <v-text-field
        v-model="search"
        append-icon="search"
        label="Search"
        single-line
        hide-details
      ></v-text-field>
    </v-card-title>

    <v-data-table
    :headers="headers"
    :items="contacts"
    sort-by="calories"
    class="elevation-1"
    :search="search"
  >

    <template v-slot:top>
      <v-toolbar flat color="white">
        <v-spacer></v-spacer>
        <v-dialog v-model="dialog" max-width="500px">
          <template v-slot:activator="{ on }">
            <v-btn color="primary" dark class="mb-2" v-on="on">New Item</v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="headline">{{ formTitle }}</span>
            </v-card-title>

            <v-card-text>
              <v-form
                ref="form"
                v-model="valid"
                lazy-validation
              >
                <v-row>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field v-model="editedItem.name" label="Name" :rules="stringRules"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-text-field v-model="editedItem.surname" label="Surname" :rules="stringRules"></v-text-field>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-radio-group v-model="editedItem.gender" label="Gender:" column 
                      :rules="[v => !!v || 'Gender is required']">
                      <v-radio label="Male" value="M"></v-radio>
                      <v-radio label="Female" value="F"></v-radio>
                    </v-radio-group>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-menu
                      ref="menu"
                      v-model="menu"
                      :close-on-content-click="false"
                      :return-value.sync="editedItem.birthdate"
                      transition="scale-transition"
                      offset-y
                      min-width="290px"
                    >
                      <template v-slot:activator="{ on }">
                        <v-text-field
                          v-model="editedItem.birthdate"
                          label="Birthdate"
                          prepend-icon="event"
                          readonly
                          v-on="on"
                          :rules="[v => !!v || 'Birthdate is required']"
                        ></v-text-field>
                      </template>
                      <v-date-picker v-model="editedItem.birthdate" no-title scrollable>
                        <v-spacer></v-spacer>
                        <v-btn text color="primary" @click="menu = false">Cancel</v-btn>
                        <v-btn text color="primary" @click="$refs.menu.save(editedItem.birthdate)">OK</v-btn>
                      </v-date-picker>
                    </v-menu>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-text-field v-model="editedItem.phone" label="Phone" :rules="phoneRules"></v-text-field>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-select :items="jobsList" label="Career" v-model="editedItem.career" 
                      :rules="[v => !!v || 'Career is required']"></v-select>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-text-field v-model="editedItem.comment" label="Comment"></v-text-field>
                  </v-col>

                </v-row>
              </v-form>
            </v-card-text>

            <v-card-actions>
              <v-btn color="blue darken-1" text @click="reset">Clean Form</v-btn>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="close">Cancel</v-btn>
              <v-btn color="blue darken-1" text @click="save">Save</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>

    <template v-slot:item.gender="{ item }">
      <v-icon :color="item.gender == 'M' ? 'blue' : 'pink'" small>person</v-icon>
    </template>

    <template v-slot:item.action="{ item }">
      <v-icon
        small
        class="mr-2"
        @click="editItem(item)"
      >
        edit
      </v-icon>
      <v-icon
        small
        @click="deleteItem(item)"
      >
        delete
      </v-icon>
    </template>
    <template v-slot:no-data>
      <v-btn color="primary" @click="getAll">Reset</v-btn>
    </template>
  </v-data-table>
  </v-card>
</template>

<script>
  import api from '@/ContactsApiService';
  export default {
    data: () => ({
      search: '',
      dialog: false,
      valid: true,
      contacts: [],
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Surname', value: 'surname' },
        { text: 'Gender', value: 'gender' },
        { text: 'Birthdate', value: 'birthdate' },
        { text: 'Phone', value: 'phone' },
        { text: 'Career', value: 'career' },
        { text: 'Comment', value: 'comment' },
        { text: 'Actions', value: 'action', sortable: false },
      ],
      editedIndex: -1,
      editedItem: {
        name: '',
        surname: '',
        gender: 'M',
        phone: '',
        birthdate: '',
        career: '',
        comment: ''
      },
      defaultItem: {
        name: '',
        surname: '',
        gender: 'M',
        phone: '',
        birthdate: '',
        career: '',
        comment: ''
      },
      jobsList: [],
      menu: false,
      stringRules: [
        v => !!v || 'Field is required',
        v => (v && v.length > 1) || 'Field must be more then 1 characters',
        v => (v && v.length < 39) || 'Field must be less than 40 characters',
      ],
      phoneRules: [
        v => !!v || 'Phone is required',
        v => /^[\+]?[(]?[0-9]+[)]?[-\s\.]?[0-9]+[-\s\.]?[0-9]+$/im.test(v) || 'Phone must be valid',
      ],
    }),

    computed: {
      formTitle () {
        return this.editedIndex === -1 ? 'New Contact' : 'Edit Contact'
      },
    },

    watch: {
      dialog (val) {
        val || this.close()
      },
    },

    async created () {
      this.getAll()
    },

    methods: {
      async getAll() {
        this.loading = true
        try {
          this.contacts = await api.getAll()
          this.jobsList = await api.getJobs()
          console.log(this.jobList)
        } finally {
          this.loading = false
        }
      },

      editItem (item) {
        this.editedIndex = this.contacts.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },

      deleteItem (item) {
        const index = this.contacts.indexOf(item)
        confirm('Are you sure you want to delete this item?') && this.contacts.splice(index, 1)
      },

      close () {
        this.dialog = false
        setTimeout(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        }, 300)
      },

      save () {
        if (this.$refs.form.validate()) {
            this.snackbar = true

            if (this.editedIndex > -1) {
              Object.assign(this.contacts[this.editedIndex], this.editedItem)
              api.update(this.editedItem.id, this.editedItem)
            } else {
              this.contacts.push(this.editedItem)
              api.create(this.editedItem)
            }

            this.close()
        }
      },
      reset () {
        this.$refs.form.reset()
      },
      resetValidation () {
        this.$refs.form.resetValidation()
      },
    },
  }
</script>
}
