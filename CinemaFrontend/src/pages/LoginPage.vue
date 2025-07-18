<script>
import UserApiMixine from '../mixines/Implementations/UserApiMixine';

export default{
    data(){
        return{
            login:"",
            password:"",
            isEmailValid:true,
            isPassowrdValid:true
        }
    },
    mixins:[
        UserApiMixine
    ],
    methods:{
        async LogIn(){

            var res = await this.LogInAsUser(this.login, this.password)

            var user = res.code == 200 && res.body != null && res.body != undefined && res.body.length >0 
            ? res.body[0]
            : null;

            if(res.code == 200){
                this.__setAccesToken(res.body.accessToken)
                this.__setRefrashToken(res.body.refrachToken)
                this.__setUserName(res.body.login)
                console.log("Success");
                this.$router.go(-1)
            }
            else if(res.code == 400)
            {
                console.log(res.text)
                this.$refs.login.innerText = res.text
                    this.isEmailValid = false;
                    this.isPassowrdValid = true;
            }
            return false
        },
        clearData(){
            this.__setAccesToken(null)
            this.__setRefrashToken(null)
            this.__setUserName(null)
        }
    },
    async mounted(){
        this.clearData();
    }
}
</script>

<template>
    <h1 class="text-center">Вход</h1>
    <form 
        class="d-flext flex-column border border-dark rounded-3 p-3 align-self-center w-md-50"
        @submit.prevent="this.LogIn()"
    >
        <div class="d-flext flex-column mb-3">
            <label for="" class="form-label fs-4">Логин</label>
            <input
                type="email"
                required
                class="form-control"
                aria-describedby="helpId"
                ref="loginInput"
                v-model="this.login"
            />
            <small id="helpId" ref="login" :hidden="this.isEmailValid" class="form-text text-danger">Help text</small>
        </div>
        <div class="d-flext flex-column mb-3">
            <label for="" class="form-label fs-4">Пароль</label>
            <input
                type="password"
                required
                class="form-control"
                minlength="2"
                aria-describedby="helpId"
                
                v-model="this.password"
            />
            <small id="helpId" ref="password" :hidden="this.isPassowrdValid" class="form-text text-danger">Help text</small>
        </div>
        <div class="d-flext flex-column mb-3">
            <button
                type="submit"
                class="btn btn-success w-100"
            >
                Войти
            </button>
        </div> 
    </form>
</template>   