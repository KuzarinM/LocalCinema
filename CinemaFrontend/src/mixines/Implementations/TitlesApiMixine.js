import ApiMixines from "../Common/ApiMixines" 

const TitlesApiMixine = {
    mixins: [ 
        ApiMixines 
    ],
    methods:{
        async GetTitlesList(search, isFilm, tags, pageSize, pageIndex){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                "/Titles",
                null,
                null,
                {
                    "search": search,
                    "isFilm": isFilm,
                    "tags": tags,
                    "pageNumber": pageIndex,
                    "pageSize": pageSize
                }
            ));
        },
        async GetTitlesListWithFull(search, isFilm, tags, pageSize, pageIndex){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                "/Titles/full",
                null,
                null,
                {
                    "search": search,
                    "isFilm": isFilm,
                    "tags": tags,
                    "pageNumber": pageIndex,
                    "pageSize": pageSize
                }
            ));
        },
        async GetTitleById(id){
            return await this.__CreateResponce(await this.__makeRequest(
                "GET",
                `/Titles/${id}`,
                null,
                null,
                null
            ));
        },
        async DeleteTitleById(id){
            return await this.__CreateResponce(await this.__makeRequest(
                "DELETE",
                `/Titles/${id}`,
                null,
                null,
                null
            ));
        },
        async ReorderTitleInsides(elements){
            return await this.__CreateResponce(await this.__makeRequest(
                "POST",
                `/Titles/order`,
                elements,
                null,
                null
            ));
        },
        async UpdateTitleData(id, titleData){
            return await this.__CreateResponce(await this.__makeRequest(
                "PATCH",
                `/Titles/${id}`,
                titleData,
                null,
                null
            ));
        }
    }
}

export default TitlesApiMixine