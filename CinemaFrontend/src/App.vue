<script>
  import 'vue-loading-overlay/dist/css/index.css'
  import { RouterLink, RouterView } from 'vue-router'
  import Loading from 'vue-loading-overlay'
  import {Tooltip} from 'bootstrap'
  import UserApiMixine from './mixines/Implementations/UserApiMixine'

  export default{
       data(){
        return{
            isLoading:false,
            timer:null,
            permitions:{
              IsAuthorised:false,
              EditGalery:false,
              ManageUser:false
            },
            username:null,
            mainKey: 1
        }
    },
    components:{      
        RouterLink,
        RouterView,
        Loading
    },
    mixins:[
      UserApiMixine
    ],
    methods:{
        StartLoading(){
            if (this.timer) 
              clearTimeout(this.timer);

            this.isLoading = true

            let me = this;

            // Устанавливаем новый таймер
            this.timer = setTimeout(() => {
              me.isLoading = false; // Автоматическое опускание
              me.timer = null; // Сбрасываем идентификатор таймера
            }, 3000); // 3000 мс = 3 секунды
        },
        StopLoading(){
            if (this.timer) {
              clearTimeout(this.timer);
              this.timer = null;
            }
            this.isLoading = false
        },
        async CheckRole(){
          var res = await this.CheckPermition(Object.keys(this.permitions))

          if(res.code == 200){
            for (let index = 0; index < Object.keys(this.permitions).length; index++) {
              const element = Object.keys(this.permitions)[index];
              if(res.body[element] == true)
                this.permitions[element] = true
              else if(res.body[element] == false)
                this.permitions[element] = false
            }
          }

          console.log(this.permitions)

          this.username = this.__getUserName()
        },
        async Logout(){
            this.__setAccesToken(null)
            this.__setRefrashToken(null)
            this.__setUserName(null)

            await this.CheckRole();

            this.mainKey += 1
        }
    },
    provide(){
      return{
        OpenLoader:  this.StartLoading,
        CloseLoader: this.StopLoading,
      }
    },
    async mounted(){
      var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
      var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new Tooltip(tooltipTriggerEl)
      })
    },
    // async beforeUpdate(){
    //   await this.CheckRole();
    // }
  }
</script>

<template>

  <nav
    class="navbar navbar-expand-sm navbar-light bg-light"
  >
    <div class="container">
      <a class="navbar-brand" href="#">OfflineCimena</a>
      <button
        class="navbar-toggler d-lg-none"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#collapsibleNavId"
        aria-controls="collapsibleNavId"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="collapsibleNavId">
        <ul class="navbar-nav me-auto mt-2 mt-lg-0">
          <li class="nav-item">
          <RouterLink 
              class="nav-link" 
                activeClass ="nav-link active" 
              :to="{ name: 'films' }"
            >
              Фильмы
            </RouterLink>
          </li>
          <li class="nav-item">
          <RouterLink 
              class="nav-link" 
                activeClass ="nav-link active" 
              :to="{ name: 'series' }"
            >
              Сериалы
          </RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink 
              class="nav-link" 
                activeClass ="nav-link active" 
              :to="{ name: 'galery' }"
              v-if="this.permitions.EditGalery"
            >
              Галерея
          </RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink 
              class="nav-link" 
                activeClass ="nav-link active" 
              :to="{ name: 'users' }"
              v-if="this.permitions.ManageUser"
            >
              Пользователи
          </RouterLink>
          </li>
        </ul>
        <div>
          <RouterLink 
              class="nav-link" 
                activeClass ="nav-link active" 
              :to="{ name: 'login' }"
              v-if="!this.permitions.IsAuthorised"
            >
              Login
            </RouterLink>
            <p 
              class="nav-link hovered-p my-auto" 
              @click="this.Logout"
              v-if="this.permitions.IsAuthorised"
            >
              Выйти ({{ this.username }})
          </p>
        </div>
      </div>
    </div>
  </nav>

  <router-view
  :key="this.mainKey"
  @vue:updated="this.CheckRole" />

  <Loading :active="isLoading" />
</template>

<style scoped>

.hovered-p:hover{
  cursor: pointer;
}

</style>
