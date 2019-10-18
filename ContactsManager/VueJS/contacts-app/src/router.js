// Vue imports
import Vue from 'vue'
import Router from 'vue-router'

// our own imports
import Hello from '@/components/Hello'
import Contacts from '@/components/Contacts'

Vue.use(Router)

let router = new Router({
    mode: 'history',
    routes: [
        {
            path: '/',
            name: 'Hello',
            component: Hello
        },
        {
            path: '/contacts',
            name: 'Contacts',
            component: Contacts,
            meta: {
                requiresAuth: true
            }
        },
    ]
})

export default router