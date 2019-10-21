import Vue from 'vue'
import './plugins/axios'
import App from './App.vue'
import router from './router'
import vuetify from './plugins/vuetify';
import 'material-design-icons-iconfont/dist/material-design-icons.css'
import '@mdi/font/css/materialdesignicons.css'

Vue.use(vuetify)
Vue.use(router)

Vue.config.productionTip = false

new Vue({
  el: '#app',
  router,
  vuetify,
  template: '<App/>',
  components: { App },
  render: h => h(App),
})
