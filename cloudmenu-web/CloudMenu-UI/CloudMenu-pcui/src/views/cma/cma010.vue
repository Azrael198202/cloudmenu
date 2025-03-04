<template>
  <div class="container">
    <div class="main" ref="form">
      <el-row type="flex" align="middle">
        <el-col :span="2"><span>メニュー名</span></el-col>
        <el-col :span="5">
          <el-input></el-input>
        </el-col>
        <el-col :span="3" :offset="2"><span>表示/非表示</span></el-col>
        <el-col :span="4">
          <el-select v-model="viewFlg">
            <el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value"></el-option>
          </el-select>
        </el-col>
      </el-row>
      <div style="margin-top: 20px">
        <el-button>
          <i class="fa fa-plus" aria-hidden="true"></i>
          新規
        </el-button>
        <el-button>
          <i class="fa fa-pencil-square-o" aria-hidden="true"></i>
          修正
        </el-button>
        <el-button>
          <i class="fa fa-trash" aria-hidden="true"></i>
          削除
        </el-button>
      </div>
      <el-table
        ref="singleTable"
        border
        :data="tableData"
        highlight-current-row
        @current-change="handleCurrentChange"
        style="width: 100%"
        height="550"
      >
        <el-table-column type="index" label="No"></el-table-column>
        <el-table-column property="menumenuBurui" label="メニュー名"></el-table-column>
        <el-table-column property="menuBurui" label="メニュー分類"></el-table-column>
        <el-table-column property="viewFlg" label="表示/非表示"></el-table-column>
        <el-table-column property="viewFlg1" label="表示/非表示"></el-table-column>
      </el-table>
      <div style="margin-top: 20px">
        <el-button>戻る</el-button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      currentRow: null,
      viewFlg: ' ',
      options: [
        {
          value: '0',
          label: '表示'
        },
        {
          value: '1',
          label: '非表示'
        }
      ],
      tableData: [
        {
          menumenuBurui: '春メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '表示',
          viewFlg1: '表示'
        },
        {
          menumenuBurui: '夏メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '表示'
        },
        {
          menumenuBurui: '秋メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '非表示'
        },
        {
          menumenuBurui: '冬メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '非表示'
        },
        {
          menumenuBurui: '春メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '表示'
        },
        {
          menumenuBurui: '夏メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '表示'
        },
        {
          menumenuBurui: '秋メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '非表示'
        },
        {
          menumenuBurui: '冬メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '非表示'
        },
        {
          menumenuBurui: '春メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '表示'
        },
        {
          menumenuBurui: '夏メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '表示'
        },
        {
          menumenuBurui: '秋メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '非表示'
        },
        {
          menumenuBurui: '冬メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '非表示'
        },
        {
          menumenuBurui: '春メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '表示'
        },
        {
          menumenuBurui: '夏メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '表示'
        },
        {
          menumenuBurui: '秋メニュー2021',
          menuBurui: '単品、ランチ、定食、飲み物、春限定',
          viewFlg: '非表示'
        }
      ]
    }
  },

  methods: {
    setCurrent(row) {
      this.$refs.singleTable.setCurrentRow(row)
    },
    handleCurrentChange(val) {
      this.currentRow = val
    }
  }
}
</script>
<style></style>
