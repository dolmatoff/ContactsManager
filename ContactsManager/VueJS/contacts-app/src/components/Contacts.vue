<template>
    <v-card class="mx-auto"
            max-width="100%">
        <v-card-title>
            Contacts table
            <v-spacer></v-spacer>
            <v-text-field v-model="search"
                          append-icon="search"
                          label="Search"
                          single-line
                          hide-details
                          :disabled="error != '' || loading"></v-text-field>
        </v-card-title>

        <v-data-table :headers="headers"
                      :items="contacts"
                      sort-by="calories"
                      class="elevation-1"
                      :search="search"
                      :loading="loading">

          <template v-slot:top>
            <v-toolbar flat color="white">
              <v-spacer></v-spacer>
              <v-dialog v-model="dialog" max-width="500px">
                <template v-slot:activator="{ on }">
                  <v-btn color="primary" dark class="mb-2" v-on="on" text 
                         :disabled="error != '' || loading">
                    New Item
                  </v-btn>
                </template>
                <v-card>
                  <v-card-title>
                    <span class="headline">{{ formTitle }}</span>
                  </v-card-title>

                  <v-card-text>
                    <ContactForm :editedItem.sync="editedItem" :jobsList="jobsList" ref="childForm"/>
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
                <v-icon small
                        class="mr-2"
                        @click="editItem(item)">
                    edit
                </v-icon>
                <v-icon small
                        @click="deleteItem(item)">
                    delete
                </v-icon>
            </template>

            <template v-slot:no-data>
                <v-col>
                    <v-alert type="error" timeout=10>
                        {{ error }}
                    </v-alert>
                    <v-btn color="primary" @click="getAll" text>Reset</v-btn>
                </v-col>
            </template>

        </v-data-table>

        <v-snackbar v-model="snackbar" vertical>
            {{ response }}
            <v-btn color="indigo"
                   text
                   @click="snackbar = false">
                Close
            </v-btn>
        </v-snackbar>
    </v-card>
</template>

<script>
  import api from '@/ContactsApiService';
  import ContactForm from './ContactForm.vue';

  export default {
    components: { ContactForm },
    data: () => ({
      snackbar: false,
      loading: true,
      error: '',
      search: '',
      dialog: false,
      contacts: [],
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Surname', value: 'surname' },
        { text: 'Gender', value: 'gender' },
        { text: 'Birthdate', value: 'birthdate', dataType: "Date" },
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
      response: '',
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
          this.error = api.errorStatus || ''
        }
        finally {
          this.loading = false
        }
      },

      editItem(item) {
        this.editedIndex = this.contacts.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },

      deleteItem (item) {
        const index = item.id
          if (confirm(`Are you sure you want to delete contact with id = ${index}?`)) {
              api.delete(index)
              this.contacts = this.contacts.filter(c => c.id != index)
          }
      },

      close () {
        this.dialog = false
        setTimeout(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        }, 3000)
      },
      childForm() {
        return this.$refs.childForm.$refs.form;
      },

      save() {
          if (this.childForm().validate()) {
              new Promise((resolve, reject) => {
                  if (this.editedIndex > -1) {
                    Object.assign(this.contacts[this.editedIndex], this.editedItem)
                    resolve(api.update(this.editedItem.id, this.editedItem))
                  }
                  else {
                    resolve(api.create(this.editedItem))
                  }
              }).then(result => {
                  this.response = result
                  this.error = api.errorStatus || ''
                this.snackbar = this.error == ''
                if (this.editedIndex < 0) {
                    this.contacts.push(result)
                  } 
              })

            this.close()
        }
      },
      reset () {
        this.childForm().reset()
      },
      resetValidation () {
        this.childForm().resetValidation()
      },
    },
  }
</script>
}
