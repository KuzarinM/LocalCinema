// https://stackoverflow.com/questions/53010064/pass-environment-variable-into-a-vue-app-at-runtime/55962511#55962511
export default {
    data(){
        return {
            __refrashUrl: "/User/refrash",
            __refrashTokenQueryFieldName:"refrashToken"
        }
    },
    methods:{
        async __makeRequest(method, path, data, headers, queries){
            var request = {
                method: method,
                headers: new Headers()
            }
            request.headers.append("Authorization", `Bearer ${this.__getAccessToken()}`)
            request.headers.append("Content-Type", "application/json")

            if(data !== null)
                request.body = JSON.stringify(data)

            if(headers !== null){
                Object.keys(headers).forEach(key => {
                    request.headers.append(key, headers[key])
                });
            }
            
            var out = [];

            if(queries != null)
                Object.keys(queries).forEach(key => {
                    if (queries[key] === undefined || queries[key] === null || queries[key] === '') {
                        delete queries[key];
                    }
                    else if(Array.isArray(queries[key])){
                        for (let index = 0; index < queries[key].length; index++) {
                            const element = queries[key][index];
                            out.push(`${key}=${element}`)
                        }
                    }
                    else{
                        out.push(`${key}=${queries[key]}`)
                    }
                }); 
                
            console.log(out)

            var url = `${window.location.origin}/proxy${path}?${out.join('&')}`

            var res = await fetch(url, request)

            if(res.status == 401 && path != this.__refrashUrl){
                await this.__refrashToken()

                res = await this.__makeRequest(method, path, data, headers, queries)
            }

            return res
        },
        async __makeFileRequest(method, path, file, headers, queries){
            var request = {
                method: method,
                headers: new Headers()
            }
            request.headers.append("Authorization", `Bearer ${this.__getAccessToken()}`)

            request.body = new FormData()

            if(file !== null)
                request.body.append('file', file)

            if(headers !== null){
                Object.keys(headers).forEach(key => {
                    request.headers.append(key, headers[key])
                });
            }
            
            var out = [];

            if(queries != null)
                Object.keys(queries).forEach(key => {
                    if (queries[key] === undefined || queries[key] === null || queries[key] === '') {
                        delete queries[key];
                    }
                    else if(Array.isArray(queries[key])){
                        for (let index = 0; index < queries[key].length; index++) {
                            const element = queries[key][index];
                            out.push(`${key}=${element}`)
                        }
                    }
                    else{
                        out.push(`${key}=${queries[key]}`)
                    }
                }); 
                
            console.log(out)

            var url = `${window.location.origin}/proxy${path}?${out.join('&')}`

            console.log(request)

            var res = await fetch(url, request)

            if(res.status == 401 && path != this.__refrashUrl){
                await this.__refrashToken()

                res = await fetch(url, request)
            }

            return res
        },
        async __CreateResponce(rawResponce){
            var res = {
                code: rawResponce.status,
                body: null,
                text:null,
                headers: null
            }

            try{
                if(res.code !== 204){
                    //res.body = await rawResponce.json()
                    res.text = await rawResponce.text()
                    res.body = JSON.parse(res.text)
                }
                
                res.headers = await rawResponce.headers
            }
            catch(ex){
                
            }

            return res;
        },
        async __CreateImageResponce(rawResponce){
            var res = {
                code: rawResponce.status,
                body: null,
                headers: null
            }

            try{
                if(res.code !== 204){
                    res.body = await rawResponce.blob()
                }
                
                res.headers = await rawResponce.headers
            }
            catch(ex){
                
            }

            return res;
        },
        __makeUrl(path, queries){

            var out = [];

            if(queries != null)
                Object.keys(queries).forEach(key => {
                    if (queries[key] === undefined || queries[key] === null || queries[key] === '') {
                        delete queries[key];
                    }
                    else if(Array.isArray(queries[key])){
                        for (let index = 0; index < queries[key].length; index++) {
                            const element = queries[key][index];
                            out.push(`${key}=${element}`)
                        }
                    }
                    else{
                        out.push(`${key}=${queries[key]}`)
                    }
                }); 

            return `${window.location.origin}/proxy${path}?${out.join('&')}`
        },
        async __refrashToken(){

            var query = {}

            query[this.__refrashTokenQueryFieldName] = this.__getRefrashToken()

            var res = await this.__CreateResponce (await this.__makeRequest(
                "GET",
                this.__refrashUrl,
                null,
                null,
                query
            ))

            console.log(res)

            if(res.code == 200){
                this.__setAccesToken(res.body.accessToken)
                this.__setRefrashToken(res.body.refrachToken)
            }
        },
        __getAccessToken(){
            return localStorage.getItem("accessToken") 
        },
        __getRefrashToken(){
            return localStorage.getItem("refrashToken") 
        },
        __getUserName(){
            return localStorage.getItem("username") 
        },
        __setAccesToken(token){

            if(token == null)
                localStorage.removeItem("accessToken")
            else
                localStorage.setItem("accessToken", token)
        },
        __setRefrashToken(token){
            if(token == null)
                localStorage.removeItem("refrashToken")
            else
                localStorage.setItem("refrashToken", token)
        },
        __setUserName(name){
            if(name == null)
                localStorage.removeItem("username")
            else
                localStorage.setItem("username", name)
        }
    }
}

