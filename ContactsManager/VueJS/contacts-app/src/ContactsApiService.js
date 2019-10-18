﻿import Vue from 'vue'
import axios from 'axios'

const client = axios.create({
    baseURL: 'https://localhost:44350/api',
    json: true
})

export default {
    async execute(method, resource, data) {
        return client({
            method,
            url: resource,
            data,
        }).then(req => {
            return req.data
        })
    },
    getAll() {
        return this.execute('get', '/contacts/')
    },
    create(data) {
        return this.execute('post', '/contacts/', data)
    },
    update(id, data) {
        return this.execute('put', `/contacts/${id}`, data)
    },
    delete(id) {
        return this.execute('delete', `/contacts/${id}`)
    },
    async getJobs(){
        return this.execute('get', '/career/')
    }
}