import ApiMixines from "../Common/ApiMixines" 

const UserApiMixine = {
    mixins: [ 
        ApiMixines 
    ],
    methods:{
        async LogInAsUser(login, password){
            return await this.__CreateResponce(await this.__makeRequest(
                "POST",
                "/User/login",
                {
                    "login":login,
                    "password":password
                },
                null,
                null
            ));
        },
        async Register(username, email, roles, password){
            return await this.__CreateResponce(await this.__makeRequest(
                "POST",
                "/User/register",
                {
                    "username":username,
                    "email":email,
                    "roles":roles,
                    "password":password
                },
                null,
                null
            ));
        },
        async Refrash(token){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                "/User/refrash",
                null,
                null,
                {
                    "refrashToken":token
                }
            ));
        },
        async CheckPermition(permitions){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                "/User/check",
                null,
                null,
                {
                    "perm":permitions
                }
            ));
        },
        async GetUserList(search, pageSize, pageIndex){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                "/User",
                null,
                null,
                {
                    "search": search,
                    "pageNumber": pageIndex,
                    "pageSize": pageSize
                }
            ));
        },
        async GetUserById(id){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                `/User/${id}`,
                null,
                null,
                null
            ));
        },
        async UpdateUserById(id, model){
            return await this.__CreateResponce(await this.__makeRequest(
                "PATCH",
                `/User/${id}`,
                model,
                null,
                null
            ));
        },
        async GetRoles(){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                "/User/roles",
                null,
                null,
                null
            ));
        }
    }
}

export default UserApiMixine