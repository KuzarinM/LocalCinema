<script>
import Multiselect from '@vueform/multiselect'

export default{
    data(){
        return{
            _pageSizeOptions:[
                1,
                10,
                25,
                50,
                100
            ],
            _windowSize:2,
            _goToPage:null,
        }
    },
    computed:{
        _totalPages:{
            get(){
                return this.TotalPages
            },
            set(value) {
                this.$emit('update:TotalPages', value)
            }
        },
        currentPage:{
            get(){
                return this.PageNumber
            },
            set(value) {
                console.log(value)
                this.$emit('update:PageNumber', value)
            }
        },
        pageSize:{
            get(){
                return this.PageSize
            },
            set(value) {
                this.$emit('update:PageSize', value)
            }
        },
        windowStartPoint(){
            return Math.max(this.currentPage - this._windowSize - 1, 1)
        },
        windowEndPoint(){
            return Math.min(this.currentPage + this._windowSize, this._totalPages-1)
        },
        filters:{
            get(){
                return this.FiltersData
            },
            set(value){
                this.$emit('update:FiltersData', value)
            }
        }
    },
    components:{
        Multiselect
    },
    props:{
        TotalPages:{ 
            type: Number, 
            required: false, 
            default: 10 
        },
        PageNumber:{ 
            type: Number, 
            required: false, 
            default: 1 
        },
        PageSize:{ 
            type: Number, 
            required: false, 
            default: 10 
        },
        UseQueries:{ 
            type: Boolean, 
            required: false, 
            default: true 
        },
        FiltersData:
        { 
            type: Array, 
            required: false, 
            default: [
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
                filterName:"startDate",
                type:"datetime-local",
                displayName:"Начиная с ",
                discription:"Начальная точка временного диапозона, за который получаем логи",
                placeholder:"",
                weight: "fit-content",
                value: null
            },
            {
                filterName:"lavels",
                type:"multi-select",
                displayName:"Уровень",
                discription:"Уровни логирования, которые будут запрашиваться",
                placeholder:"",
                options: [],
                weight:"20%",
                value: []
            }
        ] 
        }
    },
    emits: [
        'update:TotalPages', 
        'update:PageNumber',
        'update:PageSize',
        'applyFilter'
    ],
    watch: 
    {
        TotalPages(oldTotalPages, newTotalPages){
            this.SaveParams(); // Как только значение в родителе обновилось, нужно обновить его в пути
        },
        PageNumber(oldPageNumber, newPageNumber){
            this.SaveParams(); // Как только значение в родителе обновилось, нужно обновить его в пути
        },
        PageSize(oldPageSize, newPageSize){
            this.SaveParams(); // Как только значение в родителе обновилось, нужно обновить его в пути
        }
    },
    methods:{
        MoveToPage(page){
            if(page>0 && page <= this._totalPages){
                this.currentPage = page
            }
            this._goToPage=null
        },
        SaveParams(){

            this.$emit("applyFilter")
            
            if(!this.UseQueries)
            return

            var quries = JSON.parse(JSON.stringify(this.$route.query));

            for (let index = 0; index < this.filters.length; index++) {
                const element = this.filters[index];
                
                var value = JSON.parse(JSON.stringify(element.value)) // Получаем значение из прокси

                if(value != null){
                    quries[element.filterName] = value
                }
                else if (quries[element.filterName] != null){
                delete quries[element.filterName]
                }
            }

            var params = {
                pageSize:this.pageSize,
                pageNumber:this.currentPage
            }

            for (const [key, value] of Object.entries(params)) {
                if(value != null){
                    quries[key] = value
                }
                else if (quries[key] != null){
                    delete quries[key]
                }
            }

            console.log(quries);
            console.log(12);
            this.$router.push({path: this.$route.fullPath, query: quries, params: this.$route.params });
        },
        handleCheckboxChange(obj) {
            if (obj.value === null) {
                obj.value = true;
                return
            } else if (obj.value === true) {
                obj.value = false;
                return
            } else if (obj.value === false) {
                obj.value = null;
                return
            }
        }
    },
    async mounted(){
        if(this.UseQueries){
            await this.$router.isReady()

            if(this.$route.query.pageSize != null){
                let newPageSize = parseInt(this.$route.query.pageSize)

                if(newPageSize != this.pageSize)
                this.pageSize = newPageSize
            }
            if(this.$route.query.pageNumber != null){
                let newPageNumber = parseInt(this.$route.query.pageNumber)

                if(newPageNumber != this.currentPage)
                    this.currentPage = newPageNumber
            }
                
            for (let index = 0; index < this.filters.length; index++) {
                const element = this.filters[index];
                
                if(this.$route.query[element.filterName] != null ){
                    element.value = this.$route.query[element.filterName]

                    if(element.type === 'checkbox' && (typeof element.value === 'string' || element.value instanceof String)){
                        element.value = element.value == 'true'
                    }

                    if(element.type === 'multi-select' && !(typeof element.value === 'array' || element.value instanceof Array)){
                        element.value = [element.value]
                    }
                }
            }

           this.$emit("applyFilter")
        }
    }
}
</script>

<template>
    <div class="d-flex w-100">
        <hr class="hr me-2" style="width: 5%;"/>
        <span>Фильтры</span>
        <hr class="hr w-100 ms-2" />
    </div>

    <div class="d-flex flex-column flex-md-row justify-content-center">
            <div 
                class="mx-3 mb-3 d-flex flex-column text-left text-start align-self-normal" 
                v-for="item in this.filters" 
                :style="`min-width: ${item.weight};`"
            >
                <label 
                    for="" 
                    class="d-flex form-label mb-1 justify-content-start"
                >
                    <p class="me-1 mb-0">{{item.displayName}}</p>
                    
                    <i 
                        class="bi bi-info-circle mb-0" 
                        data-bs-toggle="tooltip" 
                        data-bs-placement="top" 
                        :title="item.discription" 
                    >
                    </i>
                </label>

                <input 
                    class="form-check-input mx-auto"
                    type="checkbox"
                    v-model="item.value"
                    :key="item.value"
                    :indeterminate="item.value == null"
                    @click.prevent="this.handleCheckboxChange(item)"
                    v-if="item.type == 'checkbox'"  
                    />
                <input
                    v-else-if="item.type != 'select' && item.type != 'multi-select'"
                    :type="item.type"
                    class="form-control"
                    name=""
                    id=""
                    v-model="item.value"
                    :placeholder="item.placeholder"
                />
                <Multiselect
                    v-model="item.value"
                    mode="tags"
                    :close-on-select="false"
                    :searchable="true"
                    :create-option="false"
                    :options="item.options"
                     v-else-if ="item.type == 'multi-select'"
                />
                <Multiselect
                    v-model="item.value"
                    :close-on-select="true"
                    :searchable="true"
                    :create-option="false"
                    :options="item.options"
                    v-else
                />      
            </div>
            <button
                    type="button"
                    class="btn btn-primary my-auto justify-content-start mx-3"
                    @click="this.SaveParams()"
            >
                Применить
            </button>
    </div>

    <slot>

    </slot>

     <nav 
    aria-label="Page navigation" 
    class="d-flex justify-content-center my-3"
    >
    <ul
        class="pagination my-auto"
    >
        <li 
            :class=" this.currentPage - 1 > 0 ? 'page-item' : 'page-item disabled'" 
            style="height: fit-content;"
        >
            <a class="page-link" aria-label="Previous" @click="this.MoveToPage(this.currentPage-1)">
                <span aria-hidden="true">&laquo;</span>
            </a>
        </li>

        <li 
            :class="this.currentPage == 1? 'page-item active' : 'page-item'" aria-current="page"
            style="height: fit-content;"
        >
            <a class="page-link" @click="this.MoveToPage(1)">1</a>
        </li>

        <li 
            class="page-item disabled" 
            aria-current="page" 
            v-if="this.windowStartPoint > 1"
            style="height: fit-content;"
        >
            <p class="page-link my-0">...</p>
        </li>

        <li 
            :class="(index + this.windowStartPoint) == this.currentPage? 'page-item active' : 'page-item'" 
            v-for="index in Math.max(this.windowEndPoint - this.windowStartPoint, 0)"
            style="height: fit-content;"
        >
            <a class="page-link" @click="this.MoveToPage(index + this.windowStartPoint)">{{index + this.windowStartPoint}}</a>
        </li>

        <li 
            class="page-item disabled" 
            aria-current="page" 
            v-if="this.windowEndPoint < this._totalPages-1"
            style="height: fit-content;"
        >
            <p class="page-link my-0">...</p>
        </li>


        <li 
            :class="this.currentPage == this._totalPages? 'page-item active' : 'page-item'"
            style="height: fit-content;"
            v-if="this._totalPages>1"
        >
            <a class="page-link" @click="this.MoveToPage(this._totalPages)">{{this._totalPages}}</a>
        </li>

        <li 
            :class=" this.currentPage + 1 <= this._totalPages ? 'page-item' : 'page-item disabled'"
            style="height: fit-content;"
        >
            <a class="page-link" aria-label="Next" @click="this.MoveToPage(this.currentPage+1)">
                <span aria-hidden="true">&raquo;</span>
            </a>
        </li>
    </ul>
    <select
        class="form-select form-select-sm ms-3 me-2 my-auto"
        name=""
        id=""
        style="width: fit-content; height: fit-content;"
        v-model="this.pageSize"
    >
        <option v-for="item in this._pageSizeOptions" :value="item">по {{item}}</option>
    </select>
    <div class="d-flex ">
        <p class="my-auto me-1">На страницу</p>
        <input
                v-model="this._goToPage"
                type="number"
                class="form-control form-control-sm me-1 my-auto"
                id=""
                min="1"
                :max="this._totalPages"
                style="width: fit-content; height: fit-content; width: 100px;"
                @change="this.MoveToPage(this._goToPage)"
            />
    </div>
   </nav>
</template>

<style src="@vueform/multiselect/themes/default.css"></style>