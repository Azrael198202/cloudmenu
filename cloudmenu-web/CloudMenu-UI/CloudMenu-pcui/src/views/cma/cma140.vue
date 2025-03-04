<template>
  <div class="container">
    <el-row>
      <el-col :span="12">
        <grid-layout
          :layout.sync="layout"
          :col-num="60"
          :maxRows="50"
          :row-height="2"
          :is-draggable="draggable"
          :is-resizable="resizable"
          :responsive="false"
          :vertical-compact="false"
          :prevent-collision="true"
          :useCssTransforms="true"
          style="width:730px;height:611px"
        >
          <grid-item
            v-for="item in layout"
            :key="item.label"
            :static="item.static"
            :x="item.x"
            :y="item.y"
            :w="item.w"
            :h="item.h"
            :i="item.i"
            :class="[
              item.useFlg == '0'
                ? 'akiColor'
                : item.useFlg == '1'
                ? 'yoyakuColor'
                : item.useFlg == '2'
                ? 'usingColor'
                : 'seisanColor'
            ]"
          >
            <span class="text">{{ item.i }}</span>
          </grid-item>
        </grid-layout>
      </el-col>
      <el-col :span="9" :offset="2">
        <el-row type="flex" align="middle">
          <el-col :offset="6">
            <el-table
              :data="form.tableUseInfo"
              border
              :header-cell-style="getHeaderClass"
              :cell-class-name="cellStyle"
              style="width:342px"
            >
              <el-table-column prop="type" width="80" align="center"></el-table-column>
              <el-table-column prop="akiNum" label="空き" align="center" width="65"></el-table-column>
              <el-table-column prop="yoyakuNum" label="予約済" align="center" width="65"></el-table-column>
              <el-table-column prop="siyoNum" label="使用中" align="center" width="65"></el-table-column>
              <el-table-column prop="seisanNum" label="精算済" align="center" width="65"></el-table-column>
            </el-table>
          </el-col>
        </el-row>
        <el-card style="margin-top:10px">
          <el-row type="flex" align="middle">
            <el-col :span="7" class="underLine">
              <span>座席番号</span>
              <span style="margin-left:20px">{{ form.table }}</span>
            </el-col>
            <el-col :span="3" :offset="3">
              <span>{{ form.ninsu }}</span>
            </el-col>
          </el-row>
          <el-table
            :data="tableData"
            style="width: 100%;margin-top:20px"
            height="380"
            :header-cell-style="{
              'font-weight': '500',
              color: '#606266'
            }"
          >
            <el-table-column label="注文" width="100">
              <template slot-scope="scope">
                <span>{{ scope.row.order }}</span>
              </template>
            </el-table-column>
            <el-table-column label="個数" width="48">
              <template slot-scope="scope">
                <p>{{ scope.row.num }}</p>
              </template>
            </el-table-column>
            <el-table-column label="金額" width="80">
              <template slot-scope="scope">
                <p>￥ {{ scope.row.money }}</p>
              </template>
            </el-table-column>
            <el-table-column label="備考" width="78">
              <template slot-scope="scope">
                <p>{{ scope.row.comment }}</p>
              </template>
            </el-table-column>
            <el-table-column label="厨房" width="67">
              <template slot-scope="scope">
                <el-button :class="[scope.row.tyubo == '伝票' ? 'orange-btn' : 'blue-btn']" size="mini">
                  {{ scope.row.tyubo }}
                </el-button>
              </template>
            </el-table-column>
            <el-table-column label="配膳" width="67">
              <template slot-scope="scope">
                <el-button :class="[scope.row.haizen == '済' ? 'blue-btn' : 'red-btn']" size="mini">
                  {{ scope.row.haizen }}
                </el-button>
              </template>
            </el-table-column>
            <el-table-column label="取消" width="67">
              <el-button size="mini" class="grey-btn">
                <i class="fa fa-times" aria-hidden="true"></i>
              </el-button>
            </el-table-column>
          </el-table>
          <el-row>
            <el-col :span="12" :offset="12" style="margin-top:5px">
              <el-button type="primary" plain>空</el-button>
              <el-button type="success" plain>予約済</el-button>
              <el-button type="warning" plain>使用中</el-button>
            </el-col>
          </el-row>
        </el-card>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12" :offset="1"><span class="red-font">＜小心！赶快点菜＞</span></el-col>
    </el-row>
    <el-row style="height:120px">
      <el-col :span="7" :offset="1">
        <el-table
          :data="form.haizenInfo"
          :header-cell-style="{
            'font-weight': '500',
            color: '#606266'
          }"
          :cell-class-name="cellStylehaizen"
        >
          <el-table-column prop="tableNo" label="席番号" width="105" align="left"></el-table-column>
          <el-table-column prop="foods" label="商品(料理)" align="left" width="170"></el-table-column>
          <el-table-column prop="num" label="個数" align="center" width="66"></el-table-column>
          <el-table-column prop="overtimes" label="経過時間" align="center" width="100"></el-table-column>
        </el-table>
      </el-col>
      <el-col :span="2" :offset="14"><el-button class="red-btn" plain>再新化</el-button></el-col>
    </el-row>
  </div>
</template>

<script>
import { GridLayout, GridItem } from 'vue-grid-layout'
export default {
  components: {
    GridLayout,
    GridItem
  },
  data() {
    return {
      layout: [
        { x: 0, y: 0, w: 10, h: 10, i: '個室01', useFlg: '0' },
        { x: 10, y: 0, w: 10, h: 10, i: '個室02', useFlg: '1' },
        { x: 20, y: 0, w: 10, h: 10, i: '個室03', useFlg: '3' },
        { x: 30, y: 0, w: 10, h: 10, i: '個室04', useFlg: '0' },
        { x: 40, y: 0, w: 10, h: 10, i: '個室05', useFlg: '0' },
        { x: 50, y: 0, w: 10, h: 10, i: '個室06', useFlg: '0' }
      ],
      draggable: true,
      resizable: true,
      index: 0,
      form: {
        table: '個室3',
        ninsu: '4名',
        tableUseInfo: [
          {
            type: 'ホール',
            akiNum: '1',
            yoyakuNum: '5',
            siyoNum: '7',
            seisanNum: '10'
          },
          {
            type: '個室',
            akiNum: '1',
            yoyakuNum: '4',
            siyoNum: '6',
            seisanNum: '9'
          }
        ],
        haizenInfo: [
          {
            tableNo: '06 個室',
            foods: 'キチン',
            num: '5',
            overtimes: '7'
          },
          {
            tableNo: '01ホール',
            foods: '1',
            num: '5',
            overtimes: '7'
          },
          {
            tableNo: '01ホール',
            foods: '1',
            num: '5',
            overtimes: '7'
          }
        ]
      },
      tableData: [
        {
          order: '06 水煮牛肉',
          num: '1',
          money: '999',
          comment: '大盛',
          tyubo: '伝票',
          haizen: '済'
        },
        {
          order: '06 水煮牛肉',
          num: '1',
          money: '999',
          comment: '大盛',
          tyubo: '伝票',
          haizen: '済'
        },
        {
          order: '06 水煮牛肉',
          num: '1',
          money: '999',
          comment: '大盛',
          tyubo: '伝票',
          haizen: '済'
        },
        {
          order: '06 水煮牛肉',
          num: '1',
          money: '999',
          comment: '大盛',
          tyubo: '伝票',
          haizen: '未'
        }
      ],
      value: '',
      options: [
        {
          value: '0',
          label: '表示'
        },
        {
          value: '1',
          label: '非表示'
        }
      ]
    }
  },
  mounted() {
    // this.$gridlayout.load();
    this.index = this.layout.length
  },
  methods: {
    //ヘッダー部の背景色を設定
    getHeaderClass({ row, column, rowIndex, columnIndex }) {
      if ((rowIndex === 0) & (columnIndex == 1)) {
        return 'background-color: #b4c7e7;font-weight:500;color: #606266'
      } else if ((rowIndex === 0) & (columnIndex == 2)) {
        return 'background-color: #c5e0b4;font-weight:500;color: #606266'
      } else if ((rowIndex === 0) & (columnIndex == 3)) {
        return 'background-color: #f8cbad;font-weight:500;color: #606266'
      } else if ((rowIndex === 0) & (columnIndex == 4)) {
        return 'background-color: #ffff00;font-weight:500;color: #606266'
      } else {
        return ''
      }
    },
    //セールの背景色を設定
    cellStyle({ row, column, rowIndex, columnIndex }) {
      if (columnIndex === 1) {
        // 指定列号
        return 'akiColor'
      } else if (columnIndex === 2) {
        return 'yoyakuColor'
      } else if (columnIndex === 3) {
        return 'usingColor'
      } else if (columnIndex === 4) {
        return 'seisanColor'
      } else {
        return ''
      }
    },
    //経過時間のフォント色を設定
    cellStylehaizen({ row, column, rowIndex, columnIndex }) {
      if (columnIndex === 3) {
        return 'red-font'
      } else {
        return ''
      }
    }
  }
}
</script>

<style lang="scss">
@import '@/styles/variables.scss';
* {
  -moz-user-select: none;
  -webkit-user-select: none;
  user-select: none;
}

// 一覧行の高さを低くする
.el-table--medium th,
.el-table--medium td {
  padding: 0px 0 !important;
}
//空き色
.akiColor {
  background: $blue;
}
//予約済み色
.yoyakuColor {
  background: $yoyakuColor;
}
//使用中色
.usingColor {
  background: $usingColor;
}
//清算済み色
.seisanColor {
  background: $seisanColor;
}

/*************************************/
//テーブル全体のDIV
.vue-grid-layout {
  margin-left: 50px;
  background: white;
  border: 1px solid $blue;
}
//テーブルの枠色
.vue-grid-item:not(.vue-grid-placeholder) {
  border-radius: 6px;
  border: 1px solid $blue;
}
//テーブルの様式
.vue-grid-item .text {
  font-size: 18px;
  text-align: center;
  position: absolute;
  top: 50%;
  bottom: 0;
  left: 0;
  right: 0;
  margin: auto;
  height: 75%;
  width: 100%;
}

.vue-grid-item .no-drag {
  height: 100%;
  width: 100%;
}
.vue-grid-item .minMax {
  font-size: 12px;
}
.vue-grid-item .add {
  cursor: pointer;
}
//鼠标在指定元素上才允许拖动
.vue-draggable-handle {
  position: absolute;
  width: 20px;
  height: 20px;
  top: 0;
  left: 0;
  background: url("data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' width='10' height='10'><circle cx='5' cy='5' r='5' fill='#999999'/></svg>")
    no-repeat;
  background-position: bottom right;
  padding: 0 8px 8px 0;
  background-repeat: no-repeat;
  background-origin: content-box;
  box-sizing: border-box;
  cursor: pointer;
}
</style>
