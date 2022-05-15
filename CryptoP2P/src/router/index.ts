import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";

import Lobby from '../views/Lobby.vue';
import Register from '../views/Register.vue';
import Login from '../views/Login.vue';
import Connect from '../views/Connect.vue';
import Chat from '../views/Chat.vue';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'Connect',
    component: Connect
  },
  {
    path: '/login',
    name: 'Login',
    component: Login
  },
  {
    path: '/register',
    name: 'Register',
    component: Register
  },
  {
    path: '/lobby',
    name: 'Lobby',
    component: Lobby
  },
  {
    path: '/chat',
    name: 'Chat',
    component: Chat
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;