import Vue from 'vue'
import VueI18n from 'vue-i18n'
import en from './ja.js'
import zh from './zh.js'
Vue.use(VueI18n)
const messages = {
    en: en,
    zh: zh
}

export function getLanguage() {
    const chooseLanguage = window.localStorage.getItem("language");
    if (chooseLanguage) {
        return chooseLanguage;
    }

    return 'zh-cn'
}
const i18n = new VueI18n({
    locale: getLanguage(),
    messages
})

export default i18n