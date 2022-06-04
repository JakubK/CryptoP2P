import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";

import Main from '../Main.vue';

import Lobby from '../views/Lobby.vue';
import Connect from '../views/Connect.vue';
import Chat from '../views/Chat.vue';
import Auth from '../views/Auth.vue';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'Main',
    component: Main,
    children: [
      {
        path: '',
        name: 'Connect',
        component: Connect
      },
      {
        path: '/auth',
        name: 'Auth',
        component: Auth
      },
      {
        path: '/lobby',
        name: 'Lobby',
        component: Lobby
      },
    ]
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