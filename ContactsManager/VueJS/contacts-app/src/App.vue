<template>
    <v-app>
        <v-app-bar  app
                    color="indigo"
                    dark>
            <v-toolbar-title>Manage your contacts</v-toolbar-title>
        </v-app-bar>

        <v-content>
            <v-container class="fill-height"
                         fluid>
                <router-view />
            </v-container>
        </v-content>

    </v-app>
</template>

<script>
    export default {
        name: 'app',
        data() {
            return {
                drawer: null,  
            }
        },
        async created() {
            
        },
        watch: {
            '$route': 'onRouteChange'
        },
        methods: {
            
        }
    }
</script>