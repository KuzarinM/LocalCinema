<script>
import AdminApiMixine from '../mixines/Implementations/AdminApiMixine'
import ImagesApiMixine from '../mixines/Implementations/ImagesApiMixine'

export default{
    data(){
        return {
            loadFilmFromDiskPath:"",
            loadImagesFromDiskPath:""
        }
    },
    mixins:[
        AdminApiMixine,
        ImagesApiMixine
    ],
    methods:{
        async StartLoadingFilmsFromDisk(){
            if(!confirm(`Вы уверены что вы хотите начать синхронизацию с пути ${this.loadFilmFromDiskPath}`))
                return

            await this.StartSync(this.loadFilmFromDiskPath)
        },
        async StartGenerationg(){
            if(!confirm(`Вы уверены что вы хотите начать генерацию описания?`))
                return

            await this.GenerateDescription()
        },
        async StartReordering(){
            if(!confirm(`Вы уверены что вы хотите начать пеопредление порядка?`))
                return

            await this.StartReordering()
        },
        async StartLoadingImages(){
            if(!confirm(`Вы уверены что вы хотите начать загрузку изображения из ${this.loadImagesFromDiskPath}?`))
                return

            await this.SyncImages(this.loadImagesFromDiskPath)
        },
    }
}
</script>

<template>

    <div
        class="d-flex flex-column"
    >
        <div class="d-flex w-100">
            <hr class="hr me-2" style="width: 5%;"/>
            <span class="text-nowrap">Загрузка фильмов/сериалов с диска</span>
            <hr class="hr w-100 ms-2" />
        </div>

        <form 
            ref="form"
            class="mb-3 d-flex flex-column flex-md-row text-center" 
            style="width: fit-content; align-self: center;"
        >
            <div class="m-3">
                <label for="" class="form-label">Путь на диске</label>
                <input
                    type="text"
                    class="form-control"
                    name=""
                    id=""
                    placeholder=""
                    v-model="this.loadFilmFromDiskPath"
                />
            </div>

            <button
                type="button"
                class="btn btn-primary my-auto"
                @click="this.StartLoadingFilmsFromDisk"
            >
                Загрузить
            </button>
            
            
        </form>

    </div>

    <div
        class="d-flex flex-column"
    >
        <div class="d-flex w-100">
            <hr class="hr me-2" style="width: 5%;"/>
            <span class="text-nowrap">Запустить генерацию описания</span>
            <hr class="hr w-100 ms-2" />
        </div>

        <form 
            ref="form"
            class="mb-3 d-flex flex-column flex-md-row text-center" 
            style="width: fit-content; align-self: center;"
        >
            <button
                type="button"
                class="btn btn-primary my-auto"
                @click="this.StartGenerationg"
            >
                Загрузить
            </button>
            
        </form>

    </div>

    <div
        class="d-flex flex-column"
    >
        <div class="d-flex w-100">
            <hr class="hr me-2" style="width: 5%;"/>
            <span class="text-nowrap">Запустить переопределение порядка </span>
            <hr class="hr w-100 ms-2" />
        </div>

        <form 
            ref="form"
            class="mb-3 d-flex flex-column flex-md-row text-center" 
            style="width: fit-content; align-self: center;"
        >
            <button
                type="button"
                class="btn btn-primary my-auto"
                @click="this.StartReordering"
            >
                Загрузить
            </button>
            
        </form>

    </div>

    <div
        class="d-flex flex-column"
    >
        <div class="d-flex w-100">
            <hr class="hr me-2" style="width: 5%;"/>
            <span class="text-nowrap">Загрузка фото с диска</span>
            <hr class="hr w-100 ms-2" />
        </div>

        <form 
            ref="form"
            class="mb-3 d-flex flex-column flex-md-row text-center" 
            style="width: fit-content; align-self: center;"
        >
            <div class="m-3">
                <label for="" class="form-label">Путь на диске</label>
                <input
                    type="text"
                    class="form-control"
                    name=""
                    id=""
                    placeholder=""
                    v-model="this.loadImagesFromDiskPath"
                />
            </div>

            <button
                type="button"
                class="btn btn-primary my-auto"
                @click="this.StartLoadingImages"
            >
                Загрузить
            </button>
            
            
        </form>

    </div>
</template>

<style scoped>
</style>