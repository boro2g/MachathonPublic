const config = require("./.contentful.json");
import "dotenv/config";

module.exports = {
  buildModules: ["@nuxt/typescript-build"],
  plugins: ["~/plugins/signalR.js"],
  modules: [
    "@nuxtjs/axios",
    "@nuxtjs/apollo",
    "@nuxtjs/google-fonts",
    "nuxt-vue-material"
  ],
  vueMaterial: {
    theme: "default"
  },
  googleFonts: {
    families: {
      Roboto: {
        wght: [300, 400, 500, 700],
        ital: [400]
      }
    },
    download: true
  },
  //css: ["98.css", "@/assets/scss/main.scss"],
  axios: {
    // proxyHeaders: false
  },
  apollo: {
    clientConfigs: {
      default: "~/plugins/apolloConfig.js"
    }
  },
  /*
   ** Headers of the page
   */
  env: {
    CTF_SPACE_ID: config.CTF_SPACE_ID,
    CTF_CDA_ACCESS_TOKEN: config.CTF_CDA_ACCESS_TOKEN
  },
  head: {
    title: "Machathon Mood Board",
    meta: [
      { charset: "utf-8" },
      { name: "viewport", content: "width=device-width, initial-scale=1" },
      { hid: "description", name: "description", content: "Nuxt.js project" }
    ],
    link: [{ rel: "icon", type: "image/x-icon", href: "/favicon.ico" }]
  },
  /*
   ** Customize the progress bar color
   */
  loading: { color: "#3B8070" },
  /*
   ** Build configuration
   */
  build: {
    /*
     ** Run ESLint on save
     */
    extend(config, { isDev, isClient }) {
      if (isDev && isClient) {
        config.module.rules.push({
          enforce: "pre",
          test: /\.(js|vue)$/,
          loader: "eslint-loader",
          exclude: /(node_modules)/
        });
      }

      if (isDev) {
        config.devtool = isClient ? "source-map" : "inline-source-map";
      }
    }
  }
};


