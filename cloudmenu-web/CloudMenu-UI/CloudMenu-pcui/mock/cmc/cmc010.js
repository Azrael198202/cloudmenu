module.exports = [{
    url: '/order/searchSystemMenu.do',
    type: 'post',
    response: _ => {
        return {
            status: 200,
            menuList: [{
                    menuYPosition: 0,
                    menuXPosition: 0,
                    menuId: '001',
                    menuName: '商品登録',
                    menuLink: '/cmaGoodsUp',
                    menuButtonColor: '#4685ff',
                    menuButtonIcon: 'fa-sign-in',
                    menuFontColor: '#ffffff',
                    menuUnusedFlg: '0'
                },
                {
                    menuYPosition: 0,
                    menuXPosition: 145,
                    menuId: '002',
                    menuName: '店舗情報登録',
                    menuLink: '/cmaTenpoInfoToroku',
                    menuButtonColor: '#4685ff',
                    menuButtonIcon: 'fa-shopping-basket',
                    menuFontColor: '#ffffff',
                    menuUnusedFlg: '1'
                },
                {
                    menuYPosition: 0,
                    menuXPosition: 290,
                    menuId: '003',
                    menuName: '座席登録',
                    menuLink: '/cmaTenpoInfoToroku  ',
                    menuButtonColor: '#4685ff',
                    menuButtonIcon: 'fa-cutlery',
                    menuFontColor: '#ffffff',
                    menuUnusedFlg: '0'
                },
                {
                    menuYPosition: 0,
                    menuXPosition: 435,
                    menuId: '004',
                    menuName: '区分マスタ登録',
                    menuLink: '/cmaMaster',
                    menuButtonColor: '#4685ff',
                    menuButtonIcon: 'fa-database',
                    menuFontColor: '#ffffff',
                    menuUnusedFlg: '0'
                },
                {
                    menuYPosition: 120,
                    menuXPosition: 0,
                    menuId: '005',
                    menuName: 'メニュー登録',
                    menuLink: '/cmaMenuManage',
                    menuButtonColor: '#ff9e35',
                    menuButtonIcon: 'fa-cutlery',
                    menuFontColor: '#ffffff',
                    menuUnusedFlg: '0'
                },
                {
                    menuYPosition: 240,
                    menuXPosition: 0,
                    menuId: '006',
                    menuName: '在庫一覧',
                    menuLink: '/cmaZaikoItiran',
                    menuButtonColor: '#54c336',
                    menuButtonIcon: 'fa-calendar-minus-o',
                    menuFontColor: '#ffffff',
                    menuUnusedFlg: '0'
                },
                {
                    menuYPosition: 240,
                    menuXPosition: 145,
                    menuId: '007',
                    menuName: '入庫一覧',
                    menuLink: '/cmaNyukoItiran',
                    menuButtonColor: '#54c336',
                    menuButtonIcon: 'fa-truck',
                    menuFontColor: '#ffffff',
                    menuUnusedFlg: '0'
                }
            ]
        }
    }
}]