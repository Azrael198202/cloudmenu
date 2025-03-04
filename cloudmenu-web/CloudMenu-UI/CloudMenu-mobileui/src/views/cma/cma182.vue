<template>
    <div>
        <van-sticky :offset-top="77" class="top-title">
            <van-cell :title="query.categoryKbnName" />
        </van-sticky>
        <van-list v-model="loading" :finished="finished" :finished-text="emptyText" :immediate-check="true">
            <van-cell
                style="font-size:12px;padding-left:30px;"
                v-for="(item, index) in list"
                :key="index"
                :title="item.categoryName"
                @click="changeColor($event, item)"
            />
        </van-list>
    </div>
</template>
<script>
import { searchNyukoProduct, searchNyukoMaterial, searchNyukoEquipment } from '@/api/cma/cma182'
export default {
    name: 'SelectionOfIncomingoods',
    data() {
        return {
            query: {
                itemKbn: '',
                itemKbnName: '',
                storingDate: '',
                categoryKbn: '',
                categoryKbnName: '',
                categoryKbnSon: '',
                categoryKbnNameSon: ''
            },
            titleName: '',
            loading: false,
            list: [],
            finished: false,
            element: {
                style: {
                    backgroundColor: null
                }
            }
        }
    },
    computed: {
        emptyText: function() {
            return this.list.length == 0 ? 'なし' : ''
        }
    },
    created() {
        // 获取上一个画面数据
        if (
            sessionStorage.getItem('180Query') &&
            sessionStorage.getItem('180Query') !== null &&
            sessionStorage.getItem('180Query') !== undefined
        ) {
            const query = JSON.parse(sessionStorage.getItem('180Query'))
            this.query.itemKbn = query.itemKbn
            this.query.itemKbnName = query.itemKbnName
            this.query.storingDate = query.storingDate
            this.query.categoryKbn = query.categoryKbn
            this.query.categoryKbnName = query.categoryKbnName
        }
        // 初始化
        this.init()
    },
    methods: {
        init() {
            const data = { itemKbn: this.query.categoryKbn }
            if (this.query.itemKbn === '031') {
                searchNyukoProduct(data).then(response => {
                    if (response.status === 200) {
                        this.list = response.searchList

                        this.finished = true
                    } else if (response.status === 601) {
                        this.finished = true
                    } else if (response.status === 602) {
                        this.finished = true
                    }
                })
            } else if (this.query.itemKbn === '032') {
                searchNyukoMaterial(data).then(response => {
                    if (response.status === 200) {
                        this.list = response.searchList
                        this.finished = true
                    } else if (response.status === 601) {
                        this.finished = true
                    } else if (response.status === 602) {
                        this.finished = true
                    }
                })
            } else if (this.query.itemKbn === '033') {
                searchNyukoEquipment(data).then(response => {
                    if (response.status === 200) {
                        this.list = response.searchList
                        this.finished = true
                    } else if (response.status === 601) {
                        this.finished = true
                    } else if (response.status === 602) {
                        this.finished = true
                    }
                })
            } else {
                this.finished = true
            }
        },

        changeColor($event, item) {
            if (this.element.style) {
                this.element.style.backgroundColor = ''
            }
            // 追加商品分类
            if (item.categoryKbn && item.categoryName) {
                this.query.categoryKbnSon = item.categoryKbn
                this.query.categoryKbnNameSon = item.categoryName
            }

            this.element = $event.currentTarget
            $event.currentTarget.style.backgroundColor = '#5e5b64'
            // 重新赋值
            sessionStorage.setItem('180Query', JSON.stringify(this.query))
            // 跳转画面
            this.$router.push({ path: '/Employee/EnterTheStockInQuantity' })
        }
    }
}
</script>

<style lang="scss" scoped>
@import '@/styles/variables.scss';
.top-title {
    .van-cell {
        background-color: $headerLinearBg1;
    }
}
</style>
