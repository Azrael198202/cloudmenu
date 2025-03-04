import Cookies from 'js-cookie'

const TokenKey = 'Admin-Token'
const memeberUsername = 'member-Username'
const agentUsername = 'agentUsername'

export function getToken() {
  return Cookies.get(TokenKey)
}

export function setToken(key, token) {
  return Cookies.set(key, token)
}

export function removeToken() {
  return Cookies.remove(TokenKey)
}
//记住会员用户名
export function setMemberUsername(loginForm) {
  return Cookies.set(memeberUsername, loginForm, { expires: 30 })
}
//获取记住的会员用户名
export function getMemeberUsername() {
  return Cookies.getJSON(memeberUsername)
}
//获取记住的经纪人用户名
export function getAgentUsername() {
  return Cookies.getJSON(agentUsername)
}

//记住经纪人用户名
export function setAgentUsername(loginForm) {
  return Cookies.set(agentUsername, loginForm, { expires: 30 })
}
