import Vue from 'vue'
import Router from 'vue-router'
import Contacts from '@/components/Contacts'
import NotFoundPage from '@/components/NotFoundPage'

Vue.use(Router)

let router = new Router({
    mode: 'history',
    routes: [
        {
            path: '/',
            name: 'Contacts',
            component: Contacts
        },
        {
            path: "*",
            component: NotFoundPage
        }
    ]
})

export default router