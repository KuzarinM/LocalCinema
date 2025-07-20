import ApiMixines from "../Common/ApiMixines" 

const AdminApiMixine = {
    mixins: [ 
        ApiMixines 
    ],
    methods:{
        async StartSync(dirPath){
            return await this.__CreateResponce(await this.__makeRequest(
                "Post",
                "/Admin/sync",
                null,
                null,
                {
                    "diskPath":dirPath
                }
            ));
        },
        async GenerateDescription(dirPath){
            return await this.__CreateResponce(await this.__makeRequest(
                "Post",
                "/Admin/description/generate",
                null,
                null,
                null
            ));
        },
        async ChangeTitlesInsidesOrder(){
            return await this.__CreateResponce(await this.__makeRequest(
                "Post",
                "Admin/titles/reorder",
                null,
                null,
                null
            ));
        }
    }
}

export default AdminApiMixine