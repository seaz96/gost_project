// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },
  modules: ['nuxtjs-naive-ui', '@pinia/nuxt', '@nuxt/eslint'],
  css: ['~/components/shared/assets/css/style.scss'],
  ssr: false,


  dir: {
    pages: './components/pages',
    assets: './components/shared/assets'
  },

  vite: {
    ssr: {
      noExternal: ['naive-ui', 'vueuc']
    },
    build: {
      cssCodeSplit: false,
      copyPublicDir: false
    }
  }

})