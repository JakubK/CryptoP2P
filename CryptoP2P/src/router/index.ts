import { createRouter, createWebHistory } from "vue-router";

import Lobby from '../views/Lobby.vue';

const routes = [
  {
    path: '/',
    name: 'Lobby',
    component: Lobby
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;