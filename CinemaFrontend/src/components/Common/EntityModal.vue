<script>
import BaseModal from "./BaseModal.vue"
import Multiselect from '@vueform/multiselect'

export default{
    data(){
        return{
        }
    },
    props:{
        entityName:{ 
            type: String, 
            required: true, 
            default: "" 
        },
        canBeCreate:{
            type: Boolean, 
            required: true, 
            default: false 
        },
        canBeEdit:{
            type: Boolean, 
            required: true, 
            default: true 
        },
        canBeDelete:{
            type: Boolean, 
            required: true, 
            default: true 
        },
        modalFields:{
            type: Object, 
            required: true, 
            default: {
                "Дополнительные сведения":[
                    {
                        propertyName:"id",
                        displayName:"Id",
                        description:"Идентифкатор сущности",
                        type:"string",
                        readoly:true,
                        required:true
                    },
                    {
                        propertyName:"name",
                        displayName:"Имя",
                        description:"Имя сущности",
                        type:"string",
                        readoly:false,
                        required:true
                    },
                    {
                        propertyName:"tags",
                        displayName:"Теги",
                        description:"Теги сущности",
                        type:"multi-select",
                        options:[
                            {label:"A1", value:1},
                            {label:"A2", value:2},
                            {label:"A3", value:3},
                        ],
                        canCreate:false,
                        readoly:false,
                        required:true
                    },
                ],
                "Раздел 2":[
                    {
                        propertyName:"status",
                        displayName:"Статус",
                        description:"Статус сущности",
                        type:"checkbox",
                        label: "Соединение работет",
                        readoly:false,
                    },
                    {
                        propertyName:"text",
                        displayName:"Текст",
                        description:"Текст сущности",
                        type:"textarea",
                        readoly:true,
                        rows:10
                    },
                ]
            } 
        },
        displayObject:{
            type: Object, 
            required: true, 
            default: {
                id:"1",
                name:"123",
                status:true,
                tags:["1","2"],
                text:"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                attributes: {
                    key1: "value",
                    key2: "value"
                }
            } 
        }
    },
    emits:[
        "Delete",
        "Save",
        "Create"
    ],
    components:{
        BaseModal,
        Multiselect,
    },
    watch:{
        displayObject(newDisplayObject){
            if(newDisplayObject.id != null && newDisplayObject.id != undefined){
                var quries = JSON.parse(JSON.stringify(this.$route.query));

                quries.id = this.displayObject.id

                console.log(quries)

                this.$router.push({path: this.$route.fullPath, query: quries, params: this.$route.params });
            }
        }
    },
    methods:{
        Create(){
            if(this.$refs.form.reportValidity()){

                console.log(this.object)
                this.$emit("Create", this.object)
            }
        },
        SaveChanges(){
            if(this.$refs.form.reportValidity()){
                console.log("Save")
                console.log(this.object)
                this.$emit("Save", this.object)
            }
        },
        Delete(){
            console.log("Delete")
            this.$emit("Delete", this.object)
        },
        Open(){
            this.$refs.baseModal.OpenModal()
        },
        Close(){
            this.$refs.baseModal.CloseModal()

            this.RemoveReomQuery()
        },
        RemoveReomQuery(){
            var quries = JSON.parse(JSON.stringify(this.$route.query));

            delete quries.id

            this.$router.push({path: this.$route.fullPath, query: quries, params: this.$route.params });
        },
        GetPropertyValue(object, propertyName){
            return object[propertyName]
        }
    }
}
</script>

<template>
    <BaseModal 
        ref="baseModal"
        modalSize="lg"
        @modalClose="this.RemoveReomQuery"
    >
        <template v-slot:header>
            <h5 class="modal-title" id="modalTitleId">
                {{ this.entityName }}
            </h5>
        </template>
        <template v-slot:body>
            <form 
                ref="form"
            >
                <div
                    :key="key"
                    
                    v-for="(block, key) in this.modalFields"
                >
                    <div class="d-flex w-100">
                        <hr class="hr me-2" style="width: 5%;"/>
                        <span class="text-nowrap">{{ key }}</span>
                        <hr class="hr w-100 ms-2" />
                    </div>

                    <div 
                        class="mb-3 d-flex flex-column text-left text-start" 
                        v-for="item in block" 
                        :style="`width: ${item.weight};`"
                    >
                        <label 
                            for="" 
                            class="d-flex flex-column flex-md-row form-label mb-1 justify-content-start"
                        >
                            <p class="me-1 mb-0">{{item.displayName}}</p>
                            
                            <i 
                                class="bi bi-info-circle mb-0 d-md-block d-none" 
                                data-bs-toggle="tooltip" 
                                data-bs-placement="top" 
                                :title="item.description" 
                            >
                            </i>
                           <small id="helpId" class="d-block d-md-none form-text text-muted">{{ item.description }}</small>
                            
                        </label>

                        <div
                            v-if="item.type == 'checkbox'"
                        >
                            <input 
                                class="form-check-input me-2"
                                type="checkbox"
                                v-model="this.displayObject[item.propertyName]"
                                :key="this.displayObject[item.propertyName]"
                                :indeterminate="this.displayObject[item.propertyName] == null"
                                :disabled="item.readoly"
                                :required="item.required"
                            />
                            <label class="form-check-label" for="">{{ item.label }}</label>
                        </div>
                        <textarea 
                            class="form-control" 
                            v-model="this.displayObject[item.propertyName]"
                            :rows="item.rows"
                            :readonly="item.readoly"
                            v-else-if="item.type == 'textarea'"
                            :required="item.required"
                        >

                        </textarea>
                        <input
                            v-else-if="item.type != 'select' && item.type != 'multi-select'"
                            :type="item.type"
                            class="form-control"
                            :disabled="item.readoly"
                            v-model="this.displayObject[item.propertyName]"
                            :placeholder="item.placeholder"
                            :required="item.required"
                        />
                        <Multiselect
                            v-model="this.displayObject[item.propertyName]"
                            mode="tags"
                            :close-on-select="false"
                            :searchable="true"
                            :create-option="item.canCreate"
                            :options="item.options"
                            :disabled="item.readoly"
                            v-else-if ="item.type == 'multi-select'"
                            :required="item.required"
                        />
                        <Multiselect
                            v-model="this.displayObject[item.propertyName]"
                            :close-on-select="true"
                            :searchable="true"
                            :create-option="item.canCreate"
                            :options="item.options"
                            :disabled="item.readoly"
                            :required="item.required"
                            v-else
                        />      
                    </div>
                </div>
            </form>
            


        </template>

        <template v-slot:footer>
            <button
                type="button"
                class="btn btn-success"
                @click.prevent="this.SaveChanges()"
                v-if="this.canBeEdit"
            >
                Сохранить
            </button>

            <button
                type="button"
                class="btn btn-success"
                @click.prevent="this.Create()"
                v-if="this.canBeCreate"
            >
                Создать
            </button>

            <button
                type="button"
                class="btn btn-danger"
                @click.prevent="this.Delete()"
                 v-if="this.canBeDelete"
            >
                Удалить
            </button>
        </template>
    </BaseModal>
</template>

<style src="@vueform/multiselect/themes/default.css"></style>