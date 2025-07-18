import ApiMixines from "../Common/ApiMixines" 

const ImagesApiMixine = {
    mixins: [ 
        ApiMixines 
    ],
    methods:{
        async GetImages(search, pageSize, pageIndex){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                "/Image",
                null,
                null,
                {
                    "search": search,
                    "pageNumber":pageIndex,
                    "pageSize":pageSize
                }
            ));
        },
        async GetImage(id){
            return await this.__CreateImageResponce(await this.__makeRequest(
                "GET",
                `/Image/${id}`,
                null,
                null,
                null
            ));
        },
        async UploadImage(name, isCover, file){
            return await this.__CreateResponce(await this.__makeFileRequest(
               "POST",
                "/Image",
                file,
                null,
                {
                    "name": name,
                    "isCover": isCover
                }
            ));
        },
        GetImageUrl(id){
            return this.__makeUrl(`/Image/${id}`)
        },
        async DeleteImage(id){
            return await this.__CreateResponce(await this.__makeRequest(
                "DELETE",
                `/Image/${id}`,
                null,
                null,
                null
            ));
        },
        async GetClosestImage(texts, isCover){
            return await this.__CreateImageResponce(await this.__makeRequest(
                "GET",
                "Image/closest",
                null,
                null,
                {
                    "texts": texts,
                    isCover: isCover
                }
            ));
        },
        async SyncImages(baseDir){
            return await this.__CreateResponce(await this.__makeRequest(
                "POST",
                "Image/sync",
                null,
                null,
                {
                    "baseDir": baseDir
                }
            ));
        }
    }
}

export default ImagesApiMixine