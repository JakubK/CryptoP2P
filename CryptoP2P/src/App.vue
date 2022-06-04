<template>
  <div class="common-layout">
    <el-container>
      <router-view></router-view>
      <el-footer>
        CryptoP2P Chat
        Made by <el-link href="https://github.com/JakubK" type="primary">JakubK</el-link>
      </el-footer>
    </el-container>
  </div>
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
