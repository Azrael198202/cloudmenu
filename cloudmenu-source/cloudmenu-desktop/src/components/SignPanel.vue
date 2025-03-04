<template>
  <v-container class="grey">
    <v-row no-gutters>
      <div>
        <vueSignature ref="signature" :sigOption="option"
          :w="'800px'" :h="'400px'"
        ></vueSignature>
      </div>
    </v-row>
    <v-row no-gutters class="mt-2 red--text">
      {{errMessage}}
    </v-row>
    <v-row no-gutters class="mt-3">
      <v-col class="text-center">
        <v-btn depressed color="error"
          @click="clear"
        >
          クリア
        </v-btn>
      </v-col>
      <v-col class="text-center">
        <v-btn depressed color="primary"
          @click="save"
        >
          サイン
        </v-btn>
      </v-col>
      <v-col class="text-center">
        <v-btn depressed color="error"
          @click="cancelClicked"
        >
          キャンセル
        </v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import moment from 'moment'
import vueSignature from 'vue-signature'
import SysConst from '../lwUtils/SysConst'

export default {
  name: 'SignPanel',
  components: {
    vueSignature
  },
  props: {

  },
  data () {
    return {
      errMessage: '',
      receptionNumber: '',
      option: {
        penColor: 'rgb(0, 0, 0)',
        backgroundColor: 'rgb(255,255,255)'
      }
    }
  },
  methods: {
    initForm (receptionNumber) {
      this.receptionNumber = receptionNumber
      this.errMessage = ''
      this.clear()
    },
    clear () {
      this.$refs.signature.clear()
    },
    async save () {
      var _this = this
      this.errMessage = ''
      // 署名をチェック
      var sigDatas = _this.$refs.signature.$data.sig._data
      var allPoint = 0
      for (let i = 0; i < sigDatas.length; i++) {
        const sig = sigDatas[i]
        allPoint = allPoint + sig.points.length
      }
      if (allPoint < 40) {
        this.errMessage = '署名してください。'
        return
      }
      this.addWaterMark(this.receptionNumber)
      var jpeg = _this.$refs.signature.save('image/jpeg')
      var postBody = {
        storeNumber: SysConst.storeNumber,
        receptionNumber: this.receptionNumber,
        signatureJpgStr: jpeg
      }
      await this.$lwHttp.postAsync('file/SaveSignature', postBody)
      this.$emit('signatureSaved', this.receptionNumber)
    },
    addWaterMark (receptionNumber) {
      var _this = this
      _this.$refs.signature.addWaterMark({
        text: `${receptionNumber} canceled at ${moment().format('YYYY-MM-DD hh:mm:ss')}`, // watermark text, > default ''
        style: 'fill', // fillText and strokeText,  'all'/'stroke'/'fill', > default 'fill
        x: 10, // fill positionX, > default 20
        y: 20 // fill positionY, > default 20
      })
    },
    cancelClicked () {
      this.$emit('cancelClicked')
    }
  }
}
</script>
