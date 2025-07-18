import ApiMixines from "../Common/ApiMixines" 

const TagsApiMixine = {
    mixins: [ 
        ApiMixines 
    ],
    methods:{
        async GetTagsList(pageSize, pageIndex){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                "/Tags",
                null,
                null,
                {
                    "pageNumber":pageIndex,
                    "pageSize":pageSize
                }
            ));
        }
    }
}

export default TagsApiMixine