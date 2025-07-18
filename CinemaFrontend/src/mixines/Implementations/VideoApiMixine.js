import ApiMixines from "../Common/ApiMixines" 

const VideoApiMixine = {
    mixins: [ 
        ApiMixines 
    ],
    methods:{
        async GetInformation(id){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                `/Video/info/${id}`,
                null,
                null,
                null
            ));
        },
        async SetIsSceen(id, isSceen){
            return await this.__CreateResponce(await this.__makeRequest(
                "POST",
                `/Video/${id}/sceen`,
                null,
                null,
                {
                    isSceen:isSceen
                }
            ));
        },
        GetVideoUrl(id){
            return this.__makeUrl(`/Video/${id}`,null)
        }
    }
}

export default VideoApiMixine