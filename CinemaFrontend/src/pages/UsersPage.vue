<script>
import TableComponent from '../components/Common/TableComponent.vue';
import UniversalGrid from '../components/Common/UniversalGrid.vue';
import UserApiMixine from '../mixines/Implementations/UserApiMixine';
import EntityModal from '../components/Common/EntityModal.vue';

export default{
    data(){
        return{
            data:[],
            editedUser:{},
            createdUser:{},
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
                }
            ],
            TableStructure:[
                {
                    ProperyName: "isActive",
                    DisplayName:"Статус",
                    Type:"checkbox",
                    CharLimit:null
                },
                {
                    ProperyName: "email",
                    DisplayName:"Email",
                    Type:"text",
                    CharLimit:null
                },
            ],
            TableActions:[
                {
                    IconCLass:"bi-trash3",
                    clickEmitName:"DeleteRow"
                }
            ],
            editModalFields:{
                "Основные данные":[
                    {
                        propertyName:"id",
                        displayName:"Id подключения",
                        description:"Идентифкатор Подключения",
                        type:"string",
                        readoly:true,
                    },
                    {
                        propertyName:"login",
                        displayName:"Логин",
                        description:"Логин пользователя",
                        type:"string",
                        readoly:false,
                    },
                    {
                        propertyName:"email",
                        displayName:"Email",
                        description:"Адрес электронной почты",
                        type:"email",
                        readoly:false,
                    },
                    {
                        propertyName:"isActive",
                        displayName:"Включен?",
                        description:"",
                        type:"checkbox",
                        readoly:false,
                    }
                ],
                "Роли":[
                    {
                        propertyName:"roles",
                        displayName:"Список ролей",
                        description:"",
                        type:"multi-select",
                        options:[],
                        readoly:false
                    }
                ]
            },
            createModalFields:{
                "Основные данные":[
                    {
                        propertyName:"id",
                        displayName:"Id подключения",
                        description:"Идентифкатор Подключения",
                        type:"string",
                        readoly:true,
                    },
                    {
                        propertyName:"login",
                        displayName:"Логин",
                        description:"Логин пользователя",
                        type:"string",
                        readoly:false,
                    },
                    {
                        propertyName:"email",
                        displayName:"Email",
                        description:"Адрес электронной почты",
                        type:"email",
                        readoly:false,
                    },
                    {
                        propertyName:"isActive",
                        displayName:"Включен?",
                        description:"",
                        type:"checkbox",
                        readoly:false,
                    }
                ],
                "Роли":[
                    {
                        propertyName:"roles",
                        displayName:"Список ролей",
                        description:"",
                        type:"multi-select",
                        options:[],
                        readoly:false
                    }
                ]
            }
        }
    },
    mixins:[
        UserApiMixine
    ],
    components:{
        TableComponent,
        UniversalGrid,
        EntityModal
    },
    methods:{
        async LoadData(){
            var tmp = await this.GetUserList(
                this.filter[0].value,
                this.pageData.pageSize,
                this.pageData.pageIndex-1
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

            var roles = await this.GetRoles()

            if(roles.code == 200){
                var rolsData = roles.body.map(x=>({
                        "value":x,
                        "label":x
                    }))

                this.editModalFields["Роли"][0].options = rolsData
                this.createModalFields["Роли"][0].options = rolsData
            }
        },
        async DeleteUser(model){
            console.log("Delete",this.editedUser)
        },
        async UpdateUser(model){
            console.log("Update", this.editedUser)
        },
        async AddUser(){
            console.log("Add", this.createdUser)
        },
        async OpenUser(model){
            var user = await this.GetUserById(model.id)

            if(user.code == 200){
                this.editedUser = user.body
                this.$refs.updateModal.Open()
            }
            console.log("1111")
            console.log(this.editedUser)
        },
        OpenCreateUser(){
            this.$refs.createModal.Open()
        }
    },
    async mounted(){
        await this.LoadData();
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
    <TableComponent 
        v-model:records="this.data"
        :actions="this.TableActions"
        :structure="this.TableStructure"
        :canAdd = "true"
        @RowClick="this.OpenUser"
        @add-row="this.OpenCreateUser"
        @DeleteRow="this.DeleteUser"
    />
    </UniversalGrid>

    <EntityModal
        v-model:displayObject="this.editedUser"
        :modalFields="this.editModalFields"
        entityName="Просмотр Пользователя"
        :canBeCreate="false"
        :canBeEdit="true"
        :canBeDelete="true"
        @Delete = "this.DeleteUser"
        @Save="this.UpdateUser"
        ref="updateModal"
     />

    <EntityModal
        v-model:displayObject="this.createdUser"
        :modalFields="this.createModalFields"
        entityName="Создание Пользователя"
        :canBeCreate="true"
        :canBeEdit="false"
        :canBeDelete="false"
        @Create = "this.AddUser"
        ref="createModal"
    />
</template>