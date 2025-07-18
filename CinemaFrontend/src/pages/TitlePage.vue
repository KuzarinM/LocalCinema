<script>
import TitlesApiMixine from '../mixines/Implementations/TitlesApiMixine';
import ImagesApiMixine from '../mixines/Implementations/ImagesApiMixine';
import TagsApiMixine from '../mixines/Implementations/TagsApiMixine';
import UserApiMixine from '../mixines/Implementations/UserApiMixine';
import ImageGaleryModal from '../modals/ImageGaleryModal.vue';
import Multiselect from '@vueform/multiselect'

export default{
    data(){
        return{
            model:null,
            isEditor:true,
            tags:[],
            originalOrder:{

            }
        }
    },
    mixins:[
        TitlesApiMixine,
        ImagesApiMixine,
        TagsApiMixine,
        UserApiMixine
    ],
    inject:[
        "OpenLoader",
        "CloseLoader"
    ],
    components:{
        Multiselect,
        ImageGaleryModal
    },
    methods:{
        async LoadData(){

            try{
                this.OpenLoader()

                var permitionTask = this.CheckPermition("EditTitles");

                 var res = await this.GetTitleById(this.$route.params.id)

                if(res.code == 200){
                    this.model = res.body
                }

                var permition = await permitionTask;

                console.log(permition);

                if(permition.code == 200 && permition.body.EditTitles === true){
                    this.isEditor = true;
                }
                else{
                    this.isEditor = false;
                }

                console

                if(this.isEditor){
                    var tags = await this.GetTagsList(100,0)

                    if(tags.code == 200)
                        this.tags =  tags.body.items.map(x=>({
                        "value":x.lable,
                        "label":x.lable
                    }))
                    
                    this.originalOrder = []
                    // Сохарняем порядок
                    for (let i = 0; i < this.model.seasons.length; i++) {
                        const seasone = this.model.seasons[i];

                        this.originalOrder[seasone.id] = seasone.orderindex

                        for (let j = 0; j < seasone.episodes.length; j++) {
                            const episode = seasone.episodes[j];
                            
                            this.originalOrder[episode.id] = episode.orderindex
                        }
                        
                    }

                    
                }
            }
            finally{
                this.CloseLoader()
            }

            console.log(this.originalOrder)
        },
        async Save(){
            if(!this.isEditor)
                return

            const updateModel = {
                name: this.model.name,
                description: this.model.description,
                tags: this.model.tags
            }

            try{
                this.OpenLoader()

                await this.UpdateTitleData(this.model.id, updateModel)
            }
            finally{
                this.CloseLoader()
            }

            await this.LoadData();
        },
        changeStatus(id){
            var block = this.$refs['seson'+id][0].classList;
            if(block.contains("hide")){
                block.remove("hide")
            }
            else{
                block.add("hide")
            }
        },
        async watch(id){
            this.$router.push({ name: 'watch', params: {id:id} })
        },
        getChangedOrderObjects(){
            let changet = []

             for (let i = 0; i < this.model.seasons.length; i++) {
                    const seasone = this.model.seasons[i];

                    if(this.originalOrder[seasone.id] !== seasone.orderindex)
                        changet.push({
                            id: seasone.id,
                            orderIndex: seasone.orderindex
                        })

                    for (let j = 0; j < seasone.episodes.length; j++) {
                        const episode = seasone.episodes[j];

                        if(this.originalOrder[episode.id] !== episode.orderindex)
                            changet.push({
                                id: episode.id,
                                orderIndex: episode.orderindex
                            })
                    }
                    
            }

            return changet
        },
        async saveChengetOrder(){

            try{
                this.OpenLoader()

                var models = this.getChangedOrderObjects();

                console.log(models)

                if(models.length > 0){
                    await this.ReorderTitleInsides(models)

                    await this.LoadData()
                }
            }
            finally{
                this.CloseLoader()
            }
        },
        openChangeTileImageModal(){

            if(!this.isEditor)
                return

            this.$refs.changeImageModal.OpenModal(
                this.model.tileimageid,
                {
                    isCover: false
                }
            )
        },
        openChangeCoverImageModal(){

            if(!this.isEditor)
                return

            this.$refs.changeImageModal.OpenModal(
                this.model.coverimageid,
                {
                    isCover: true
                }
            )
        },
        async UpdateImage(data){

            try{
                this.OpenLoader()

                if(data.metadata.isCover === true){
                    await this.UpdateTitleData(this.model.id,{
                        coverimageid:data.imageId
                    })
                }
                else if(data.metadata.isCover === false){
                    await this.UpdateTitleData(this.model.id,{
                        tileimageid:data.imageId
                    })
                }
                else{
                    return
                }
            }
            finally{
                this.CloseLoader()
            }

            this.$refs.changeImageModal.Close()

            await this.LoadData()
        },
        async changeTitleType(){

            if(!this.isEditor)
                return

            try{
                this.OpenLoader()

                await this.UpdateTitleData(this.model.id,{
                            isfilm: !this.model.isfilm
                        })
            }
            finally{
                this.CloseLoader()
            }

            await this.LoadData();
        },
        async deleteTitle(){

            if(!this.isEditor)
                return

            if(!confirm("Вы уверены, что хотите удалить произведение?"))
                return

            try{
                this.OpenLoader()

                alert('Почему-то вы нажали на удаление')
                //await this.DeleteTitleById(this.model.id)

                this.$router.go(-2)
            }
            finally{
                this.CloseLoader()
            }


        }
    },
    async mounted(){
        await this.LoadData();
    }
}
</script>

<template>

    <div class="d-flex flex-column " v-if="this.model!=null">
        <div class="d-flex w-100 long-image-container mb-3">
            <img 
                class="long-image" 
                :src="this.GetImageUrl(this.model.tileimageid)" 
                @click="this.openChangeTileImageModal"
            />
        </div>

        <div class="d-flex flex-column flex-md-row justify-content-center mx-3">
            <img 
                :class="`tile-image align-self-center ${this.model.isSeen ? 'hover-image': ''}`"  
                :src="this.GetImageUrl(this.model.coverimageid)"
                @click="this.openChangeCoverImageModal"
            />
            <div class="d-flex flex-column mx-3 w-100">
                <h1>{{ this.model.isfilm ? 'Фильм' : 'Сериал' }}</h1>
                <div 
                    v-if="!this.isEditor"
                    class="mb-2"
                >
                    <h1 >{{ this.model.name }}</h1>
                </div>
                <div 
                    v-else
                    class="mb-2"
                >
                    <label 
                        for="" 
                        class="d-flex form-label mb-1 justify-content-start"
                    >
                        <p class="me-1 mb-0">Название</p>
                    </label>

                     <input
                        type="text"
                        class="form-control"
                        v-model="this.model.name"
                        width=""
                    />
                </div>

                <div 
                    v-if="!this.isEditor"
                    class="mb-2"
                >
                    <h4 class="text-start">{{ this.model.description }}</h4>
                </div>
                <div 
                    v-else
                    class="mb-2"
                >
                    <label 
                        for="" 
                        class="d-flex form-label mb-1 justify-content-start"
                    >
                        <p class="me-1 mb-0">Название</p>
                    </label>
                    <textarea 
                        class="form-control" 
                        v-model="this.model.description">
                    </textarea>
                </div>

                
                <div 
                    v-if="!this.isEditor"
                    class="d-flex flex-wrap mb-2"
                >
                    <p class="tag-outfit me-2 mb-2" v-for="tag in this.model.tags">{{ tag }}</p>
                </div>
                <div
                    v-else
                    class="mb-2"
                >
                    <label 
                        for="" 
                        class="d-flex form-label mb-1 justify-content-start"
                    >
                        <p class="me-1 mb-0">Теги</p>
                    </label>
                    <Multiselect
                        v-model="this.model.tags"
                        mode="tags"
                        :close-on-select="false"
                        :searchable="true"
                        :create-option="true"
                        :options="this.tags"
                    />
                </div>

                <div 
                    v-if="this.isEditor"
                    
                >
                    <label 
                        for="" 
                        class="d-flex form-label mb-1 justify-content-start"
                    >
                        <p class="me-1 mb-0">Действия</p>
                    </label>

                    <div
                        class="d-flex flex-md-row flex-column"
                    >
                        <button
                            type="button"
                            class="btn btn-success me-3 mb-3"
                            @click="this.Save"
                        >
                            Сохранить
                        </button>

                        <button
                            type="button"
                            class="btn btn-warning  me-3 mb-3"
                            @click="this.saveChengetOrder"
                        >
                            Сохранить изменения порядка
                        </button>

                        <button
                            type="button"
                            class="btn btn-info  me-3 mb-3"
                            @click="this.changeTitleType"
                            v-if="(this.model.isfilm && this.model.seasons.length >= 1) || !this.model.isfilm"
                        >
                            {{this.model.isfilm ? 'Сделать сериалом' : ' Сделать фильмом' }}
                        </button>

                        <button
                            type="button"
                            class="btn btn-danger  me-3 mb-3"
                            @click="this.deleteTitle()"
                        >
                            Удалить
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div v-if="this.model.lastSceenEpisode != null">
            <button
                type="button"
                class="btn btn-primary w-100 m-3 "
                @click="this.watch(this.model.lastSceenEpisode.episodeId)"
            >
                Продолжить просмотр ({{ `${this.model.lastSceenEpisode.seasoneName} ${this.model.lastSceenEpisode.episodeName}` }})
            </button>
            
        </div>

        <div
            v-if="!this.model.isfilm"
        >
            <div class="table-responsive m-3">
                <table class="table table-striped table-primary align-middle">
                    <thead class="table-light">
                        <tr>
                            <th
                                width="5%"
                            >
                                
                            </th>
                            <th
                                class="text-start"
                            >
                                Имя
                            </th>
                            <th
                                class="text-start"
                                width="10%"
                            >
                                Действия
                            </th>
                        </tr>
                    </thead>
                    <tbody class="table-group-divider" v-for="(seasone, index) in this.model.seasons">
                        <tr class="table-primary" >
                            <td>
                                <a 
                                    type="button" 
                                    @click="this.changeStatus(index)"
                                    class="h3"
                                >
                                    >
                                </a>
                            </td>
                            <td 
                                class="text-start"
                            >
                                <h3>{{seasone.name}}</h3>
                            </td>
                            <td 
                                width="10%" 
                            >
                                <input
                                    type="number"
                                    class="form-control"
                                    v-model="seasone.orderindex"
                                    width=""
                                    v-if="this.isEditor"
                                />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table 
                                class="hide table table-striped table-hover table-borderless table-primary align-middle" 
                                :ref="'seson'+index">
                                    <tbody>
                                        <tr 
                                            class="table-primary"
                                            v-for="episode in seasone.episodes"
                                        >
                                            <td width="5%"></td>
                                            <td 
                                                scope="row" 
                                                @click="this.watch(episode.id)"
                                            >
                                                <h4 class="text-start">
                                                    {{episode.name}}
                                                </h4>
                                            </td>
                                            <td 
                                                width="10%" 
                                            >
                                                <input
                                                    type="number"
                                                    class="form-control"
                                                    v-model="episode.orderindex"
                                                    width=""
                                                    v-if="this.isEditor"
                                                />
                                            </td>
                                        </tr>
            
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        
        </div>
        <div
            v-else-if="this.model.seasons.length > 0"
        >
            <div class="table-responsive m-3">
                <table class="table table-striped table-primary align-middle">
                    <thead class="table-light">
                        <tr>
                            <th
                                width="5%"
                            >
                                
                            </th>
                            <th
                                class="text-start"
                            >
                                Имя
                            </th>
                            <th
                                class="text-start"
                                width="10%"
                            >
                                Действия
                            </th>
                        </tr>
                    </thead>
                    <tbody class="table-group-divider">
                        <tr 
                                class="table-primary"
                                v-for="episode in this.model.seasons[0].episodes"
                            >
                                <td width="5%"></td>
                                <td 
                                    scope="row" 
                                    @click="this.watch(episode.id)"
                                >
                                    <h4 class="text-start">
                                        Серия {{episode.name}}
                                    </h4>
                                </td>
                                <td 
                                    width="10%" 
                                >
                                    <input
                                        type="number"
                                        class="form-control"
                                        v-model="episode.orderindex"
                                        width=""
                                        v-if="this.isEditor"
                                    />
                                </td>
                            </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div v-else>
            <button
                type="button"
                class="btn btn-primary w-100 m-3 "
                @click="this.watch(this.model.id)"
            >
                Посмотреть
            </button>
            
        </div>

        <ImageGaleryModal 
            ref="changeImageModal"
            @-save-image="this.UpdateImage"
        />
    </div>


</template>


<style>

.long-image{
    object-fit: cover; 
    object-position: top center;
    width: 100%;
}

.long-image-container{
    max-height: 300px; 
}

.tile-image{
    max-width: 300px;
    border:solid;
    border-color: black;
    border-radius: 20px;
}

.tag-outfit{
    border: solid;
    border-color: black;
    border-radius: 10px;
}

.m-w-30{
    min-width: 30%;
}

.hide{
    display: none;
}

.hover-image{
    opacity:0.4
}
</style>