<script>
import VideoApiMixine from "../mixines/Implementations/VideoApiMixine"

export default{
    data(){
        return{
            videoMetadata:null
        }
    },
    mixins:[
        VideoApiMixine
    ],
    inject:[
        "OpenLoader",
        "CloseLoader"
    ],
    methods:{
        async LoadData(){
            try{
                this.OpenLoader()

                var data = await this.GetInformation(this.$route.params.id)

                if(data.code == 200){
                    this.videoMetadata = data.body
                }
                console.log(data)
            }
            finally{
                this.CloseLoader()
            }
        },
        async VideoSetIsSceen(flag){
            try{
                this.OpenLoader()

                var res = await this.SetIsSceen(this.$route.params.id, flag)

                if(res.code == 200 || res.code == 204){
                    this.$router.go(-1);
                }
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
    <div 
        v-if="this.videoMetadata != null"
        class="d-flex flex-column"
    >
        <h1>{{ this.videoMetadata.titleName }}</h1>
        <h2>{{ this.videoMetadata.episodeName }}</h2>
        
        <video 
            width="640" 
            controls
            class="m-3 align-self-center"
        >
            <source :src="this.GetVideoUrl(this.$route.params.id)" type="video/mp4">
        </video>

        <div
            class="d-flex flex-md-row flex-column justify-content-center"
        >
            <a
                name=""
                id=""
                :class="`btn btn-primary me-3 mb-3 ${this.videoMetadata.preveousId == null? 'disabled' : ''}`"
                :href="`/watch/${this.videoMetadata.preveousId}`"
                role="button"
            >
                Назад
            </a>

            <button
                type="button"
                class="btn btn-primary me-3 mb-3"
                @click="this.VideoSetIsSceen(false)"
            >
                Сделать не просмотреным и вернуться
            </button>
            

            <a
                name=""
                id=""
                 :class="`btn btn-primary me-3 mb-3 ${this.videoMetadata.nextId == null? 'disabled' : ''}`"
                :href="`/watch/${this.videoMetadata.nextId}`"
                role="button"
                >Вперёд</a
            >
        </div>
    </div>
</template>