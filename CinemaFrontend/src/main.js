import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"
import { createMemoryHistory, createWebHistory, createRouter } from 'vue-router'
import TitlesPage from './pages/TitlesPage.vue'
import TitlePage from './pages/TitlePage.vue'
import PlayerPage from './pages/PlayerPage.vue'
import ImageGalery from './components/Common/ImageGalery.vue'
import LoginPage from './pages/LoginPage.vue'
import UsersPage from './pages/UsersPage.vue'

const routes = [
    { path: '/', redirect:"films"},
    { 
      path: '/films', 
      name:"films", 
      component: TitlesPage,
      meta: { 
        isFilm: true 
      }
    },
    { 
      path: '/series', 
      name:"series", 
      component: TitlesPage,
      meta: { 
        isFilm: false 
      }
    },
    { 
      path: '/title/:id', 
      name:"title", 
      component: TitlePage,
    },
    { 
      path: '/watch/:id', 
      name:"watch", 
      component: PlayerPage,
    },
    { 
      path: '/galery', 
      name:"galery", 
      component: ImageGalery,
    },
    { 
      path: '/login', 
      name:"login", 
      component: LoginPage,
    },
    { 
      path: '/users', 
      name:"users", 
      component: UsersPage,
    },
    // { path: '/connections', name:"connections", component: ConnectionsPage},
    // { path: '/chanels', name:"chanels", component: ChanelsPage},
    // {path:'/about', name:"about", component:AboutPage}
  ]

  const router = createRouter({
    //history: createMemoryHistory(),
    history: createWebHistory(),
    routes
  })

createApp(App).use(router).mount('#app')
