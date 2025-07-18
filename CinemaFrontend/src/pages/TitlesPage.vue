<script>
import UniversalGrid from "../components/Common/UniversalGrid.vue"
import TagsApiMixine from "../mixines/Implementations/TagsApiMixine"
import TitlesApiMixine from "../mixines/Implementations/TitlesApiMixine"
import ImagesApiMixine from "../mixines/Implementations/ImagesApiMixine"

export default{
    data(){
        return {
            data:[],
            pageData:{
                pageIndex: 1,
                pageCount: 1,
                pageSize: 10
            },
            filter:[
                {
                    filterName:"search",
                    type:"text",
                    displayName:"Поиск",
                    discription:"Поиск по содержимому названия",
                    placeholder:"Введите поиск",
                    weight: "30%",
                    value: null
                },
                {
                    filterName:"tags",
                    type:"multi-select",
                    displayName:"Теги",
                    discription:"Теги фильмов",
                    placeholder:"",
                    options: [],
                    weight:"20%",
                    value: []
                }
            ]
        }
    },
    components:{
        UniversalGrid
    },
    inject:[
        "OpenLoader",
        "CloseLoader"
    ],
    mixins:[
        TagsApiMixine,
        TitlesApiMixine,
        ImagesApiMixine
    ],
    methods:{
        async LoadData(){

            try{
                this.OpenLoader()

                var tags = await this.GetTagsList(100,0)

                if(tags.code == 200){
                    this.filter[1].options = tags.body.items.map(x=>({
                        "value":x.lable,
                        "label":x.lable
                    }))
                }


                var tmp = await this.GetTitlesList(
                    this.filter[0].value,
                    this.$route.meta.isFilm,
                    this.filter[1].value,
                    this.pageData.pageSize,
                    this.pageData.pageIndex -1
                )

                if(tmp.code == 200){
                    if(this.pageData.pageCount != tmp.body.totalPages)
                        this.pageData.pageCount = tmp.body.totalPages

                    if(this.pageData.pageIndex >= tmp.body.totalPages)
                        this.pageData.pageIndex = tmp.body.totalPages
                    
                    if(this.pageData.pageIndex <= 0 )
                        this.pageData.pageIndex = 1

                    this.data = tmp.body.items
                }
            }
            finally{
                this.CloseLoader()
            }
            
        }
    }
}
</script>

<template>
    <UniversalGrid
        v-model:-page-number="this.pageData.pageIndex"
        v-model:-page-size="this.pageData.pageSize"
        v-model:-total-pages="this.pageData.pageCount"
        v-model:-filters-data="this.filter"
        v-on:update:page-number="this.LoadData"
        v-on:update:page-size="this.LoadData"
        v-on:apply-filter="this.LoadData"
        :key="this.$route.meta.isFilm"
    >
        <div class="d-flex flex-wrap mt-3">

            
            <router-link
                v-for="item in this.data"
                style="width: 200px;"
                class="d-flex flex-column mx-3 mb-3 box no-link "
                :to="`/title/${item.id}`"
            >
                <img 
                :class="`mt-0 image-cover ${item.isSeen? 'hover-image' : ''}`" 
                height="300px"
                :src="this.GetImageUrl(item.coverimageid)" />
                <p 
                class="text-center mb-0 mt-auto p-1"
                >
                    {{ item.name.replaceAll("."," ") }}
                </p>
            </router-link>
        </div>

    </UniversalGrid>
</template>

<style scoped>

    .box{
        border: solid;
        border-radius: 20px;
        border-color: black;
    }

    .image-cover{
        border-radius: 16px 16px 0px 0px;
        object-fit: cover; 
        object-position: top center;
        width: 100%;
    }

    .no-link {
        color:inherit;
        text-decoration:none;
    }

    .hover-image{
        opacity:0.4
    }
</style>