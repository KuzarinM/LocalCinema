<script>
import ImagesApiMixine from '../../mixines/Implementations/ImagesApiMixine';
import UniversalGrid from './UniversalGrid.vue';

export default{
    data(){
        return{
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
            ],
            uploadImageData:null,
            imageModel:{
                name:"",
                isCoder:false
            }
        }
    },
    mixins:[
        ImagesApiMixine
    ],
    components:{
        UniversalGrid
    },
    props:{
        selectedImageId:{
            type:String,
            required: false, 
            default: null 
        }
    },
    computed:{
        SelectedImageId:{
             get(){
                return this.selectedImageId
            },
            set(value) {
                this.$emit('update:selectedImageId', value)
            }
        }
    },
    methods:{
        async LoadData(){
            var images = await this.GetImages(
                this.filter[0].value, 
                this.pageData.pageSize, 
                this.pageData.pageIndex-1
            )

            if(images.code == 200){
                this.data = images.body.items

                if(this.pageData.pageCount != images.body.totalPages)
                    this.pageData.pageCount = images.body.totalPages

                if(this.pageData.pageIndex >= images.body.totalPages)
                    this.pageData.pageIndex = images.body.totalPages
                
                if(this.pageData.pageIndex <= 0 )
                    this.pageData.pageIndex = 1
                
            }

            this.showImage();

        },
        showImage(){
            var files = this.$refs.imageUploader.files;
            // FileReader support
            if (FileReader && files && files.length) {
                var fr = new FileReader();

                var me = this;

                fr.onload = function () {
                    me.uploadImageData = fr.result
                }
                fr.readAsDataURL(files[0]);
            }
            else{
                this.uploadImageData = null;
            }
        },
        async addImage(){
            console.log(this.imageModel)

            await this.UploadImage(this.imageModel.name, this.imageModel.isCoder, this.$refs.imageUploader.files[0])
            
            await this.LoadData();
        },
        async deleteImage(model){
            if(!confirm(`Вы уверены, что хотите удалить изображение ${model.name}`))
                return

            await this.DeleteImage(model.id)

            await this.LoadData();
        },
        async openImage(model){
            window.open(this.GetImageUrl(model.id), '_blank')
        },
        async selectImage(model){
            this.SelectedImageId = model.id
        }
    },
    async mounted(){
        await this.LoadData()
    }
}
</script>

<template>
    <div class="d-flex flex-column">
        <div class="d-flex flex-md-row flex-column mx-3">
            <div class="mb-3 me-3">
                <label for="" class="form-label">Выберете фото</label>
                <input
                    type="file"
                    class="form-control"
                    @change="this.showImage"
                    ref="imageUploader"

                />
            </div>
            <div class="mb-3 me-3">
                <label for="" class="form-label">Отображаемое имя</label>
                <input
                    type="text"
                    class="form-control"
                    aria-describedby="helpId"
                    v-model="this.imageModel.name"
                />
            </div>

            <div class="form-check my-auto">
                <input
                    class="form-check-input"
                    type="checkbox"
                    v-model="this.imageModel.isCoder"
                    value=""
                    id=""
                    checked
                />
                <label class="form-check-label" for=""> Обложка для фильма</label>
            </div>
            
            
            <button
                type="button"
                class="btn btn-primary my-auto mx-3"
                @click="this.addImage"
            >
                Загрузить
            </button>
        </div>
       
        <div 
            class="mb-3"
            v-if="this.uploadImageData != null"
        >
            <img 
                height="300px"
                class="image-cover m-3"
                ref="imageViewer"
                :src="this.uploadImageData"
            />

        </div>
    </div>

    <UniversalGrid
        v-model:-page-number="this.pageData.pageIndex"
        v-model:-page-size="this.pageData.pageSize"
        v-model:-total-pages="this.pageData.pageCount"
        v-model:-filters-data="this.filter"
        v-on:update:page-number="this.LoadData"
        v-on:update:page-size="this.LoadData"
        v-on:apply-filter="this.LoadData"
        :-use-queries="false"
    >
    <div
        class="d-flex flex-wrap"
    >
        <div 
            v-for="image in this.data" 
            :class="`m-3 align-items-start ${this.SelectedImageId == image.id ? 'selected-image-cover' : 'image-cover'}`"
        >
            <img 
                height="300px"
                :src="this.GetImageUrl(image.id)"
                @click="this.selectImage(image)"
            />
            <p 
                class="text-center mb-0 mt-auto p-1"
                @dblclick="this.deleteImage(image)"
            >
                {{ image.name }}
            </p>
        </div>
    </div>

    </UniversalGrid>
</template>

<style>
    .image-cover{
        border:solid;
        border-color: black;
    }

    .selected-image-cover{
        border:solid;
        border-color: green;
    }
</style>