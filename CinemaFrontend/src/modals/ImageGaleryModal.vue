<script>
import ImageGalery from '../components/Common/ImageGalery.vue';
import BaseModal from '../components/Common/BaseModal.vue';

export default{
    data(){
        return{
            selectedImageId:null,
            metadata:{}
        }
    },
    components:{
        ImageGalery,
        BaseModal
    },
    emits:[
        "SaveImage"
    ],
    methods:{
        async saveImage(){
            console.log(this.selectedImageId)
            var model = {
                imageId: this.selectedImageId,
                metadata: this.metadata
            }
            this.$emit("SaveImage", model)
        },
        OpenModal(oldImageId, metadata){
            this.selectedImageId = oldImageId
            this.metadata = metadata

            this.$refs.baseModal.OpenModal()
        },
        Close(){
            this.$refs.baseModal.CloseModal()
        }
    },
    async mounted(){

    }
}
</script>

<template>

    <BaseModal
        modalSize="lg"
        ref="baseModal"
    >
        <template v-slot:header>
            <h5 class="modal-title" id="modalTitleId">
                Изменение изображения
            </h5>
        </template>

        <template v-slot:body>
            <ImageGalery
            v-model:selectedImageId="this.selectedImageId"
            >
            </ImageGalery>
        </template>

        <template v-slot:footer>
            <button
                type="button"
                class="btn btn-success"
                @click="this.saveImage"
            >
                Сохранить
            </button>
            
        </template>

    </BaseModal>

</template>