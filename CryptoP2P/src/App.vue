<template>
  <div>
    <router-link to="/connect">Connect</router-link>
    <router-link to="/login">Login</router-link>
    <router-link to="/register">Register</router-link>
  </div>
  <router-view/>
</template>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
