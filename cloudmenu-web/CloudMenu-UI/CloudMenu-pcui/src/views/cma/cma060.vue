<template>
  <div class="container">
    <el-row>
      <el-col :span="2" :offset="19"><el-button class="red-btn" @click="addItem" plain>新規</el-button></el-col>
    </el-row>
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
            <span class="remove" @click="removeItem(item.i)">x</span>
          </grid-item>
        </grid-layout>
      </el-col>
      <el-col :span="6" :offset="2">
        <el-row style="margin-top:10px;">
          <el-col :span="24" type="flex" align="middle" size="medium">
            <el-tag>座席一覧</el-tag>
          </el-col>
        </el-row>
        <el-card style="margin-top:10px">
          <el-table :data="form.tableData">
            <el-table-column type="index" width="40"></el-table-column>
            <el-table-column label="種類" width="120">
              <template slot-scope="scope">
                <el-select size="medium">
                  <el-option
                    v-for="item in scope.row.tableTypeList"
                    :key="item.value"
                    :label="item.label"
                    :value="item.tableType"
                  ></el-option>
                </el-select>
              </template>
            </el-table-column>
            <el-table-column label="席数" width="60">
              <template slot-scope="scope">
                <el-input size="medium">{{ scope.row.sekiNumber }}</el-input>
              </template>
            </el-table-column>
            <el-table-column label="" width="25">席</el-table-column>
            <el-table-column label="" width="20">×</el-table-column>
            <el-table-column label="サイズ" width="80">
              <template slot-scope="scope">
                <el-input size="medium">{{ scope.row.size }}</el-input>
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="2" :offset="12"><el-button class="red-btn" plain>保存</el-button></el-col>
      <el-col :span="2"><el-button class="red-btn" plain>削除</el-button></el-col>
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
        tableData: [
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          },
          {
            tableType: 'B1',
            tableTypeList: [
              {
                value: 'B1',
                label: 'ホール'
              },
              {
                value: 'B2',
                label: '個室'
              }
            ],
            sekiNumber: '',
            size: ''
          }
        ]
      },

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
    },
    //添加新桌子
    addItem: function() {
      // Add a new item. It must have a unique key!
      this.layout.push({
        x: (this.layout.length * 6) % (this.colNum || 60),
        y: this.layout.length + (this.colNum || 12), // puts it at the bottom
        w: 10,
        h: 10,
        i: this.index
      })
      // Increment the counter to ensure key is always unique.
      this.index++
    },
    //删除桌子
    removeItem: function(val) {
      const index = this.layout.map(item => item.i).indexOf(val)
      this.layout.splice(index, 1)
    }
  }
}
</script>

<style lang="scss">
@import '@/styles/variables.scss';

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
//坐席一览 baio
.el-tag--medium {
  height: 35px;
  width: 100%;
  font-size: 14px;
}
/*************************************/
//テーブル全体のDIV
.vue-grid-layout {
  margin-left: 50px;
  background: white;
  border: 1px solid $blue;
  -moz-user-select: none;
  -webkit-user-select: none;
  user-select: none;
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
.remove {
  position: absolute;
  right: 2px;
  top: 0;
  cursor: pointer;
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
