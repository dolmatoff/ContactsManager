<template>
  <v-form ref="form"
          v-model="valid"
          lazy-validation>
    <v-row>
      <v-col cols="12" sm="6" md="4">
        <v-text-field v-model="editedItem.name" label="Name" :rules="stringRules"></v-text-field>
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
        <v-menu ref="menu"
                v-model="menu"
                :close-on-content-click="false"
                transition="scale-transition"
                offset-y
                max-width="290px"
                min-width="290px">
          <template v-slot:activator="{ on }">
            <v-text-field v-model="editedItem.birthdate"
                          label="Birthdate"
                          hint="YYYY-MM-DD format"
                          persistent-hint
                          prepend-icon="event"
                          v-on="on"
                          :rules="dateRules"></v-text-field>
          </template>
          <v-date-picker v-model="editedItem.birthdate" no-title @input="menu = false" 
                         :min="minDate" :max="maxDate"></v-date-picker>
        </v-menu>
      </v-col>

      <v-col cols="12" sm="6" md="4">
        <v-text-field v-model="editedItem.phone" label="Phone" :rules="phoneRules"></v-text-field>
      </v-col>

      <v-col cols="12" sm="6" md="4">
        <v-select :items="jobsList" label="Career" v-model="editedItem.career"
                  :rules="[v => !!v || 'Career is required']"></v-select>
      </v-col>

      <v-col cols="12" sm="12" md="12">
        <v-textarea solo
                    name="comment"
                    label="Comment"
                    v-model="editedItem.comment"></v-textarea>
      </v-col>

    </v-row>
  </v-form>
</template>

<script>
  export default {
    name: 'ContactForm',
    props: ['jobsList', 'editedItem'],
    data: d => ({
      stringRules: [
        v => !!v || 'Field is required',
        v => (v && v.length > 1) || 'Field must be more then 1 characters',
        v => (v && v.length < 39) || 'Field must be less than 40 characters',
      ],
      phoneRules: [
        v => !!v || 'Phone is required',
        v => /^[\+]?[(]?[0-9]+[)]?[-\s\.]?[0-9]+[-\s\.]?[0-9]+$/im.test(v) || 'Phone must be valid',
      ],
      dateRules: [
        v => !!v || 'Birthdate is required',
        v => /^\d{4}-(((0)[0-9])|((1)[0-2]))-([0-2][0-9]|(3)[0-1])$/.test(v) || 'Incorrect date format',
        v => !isNaN(Date.parse(v)) || 'Incorrect date format'
      ],
      valid: true,
      menu: false,
      minDate: new Date(new Date().setFullYear(new Date().getFullYear() - 100)).toISOString().substr(0, 10),
      maxDate: new Date().toISOString().substr(0, 10),
    }),
  }
</script>
