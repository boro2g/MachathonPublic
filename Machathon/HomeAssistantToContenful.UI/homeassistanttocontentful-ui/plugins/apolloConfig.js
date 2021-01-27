export default (context) => {
  return {
    httpEndpoint: `https://graphql.contentful.com/content/v1/spaces/${process.env.CTF_SPACE_ID}`,

    getAuth: () => `Bearer ${process.env.CTF_CDA_ACCESS_TOKEN}`,
  }
}


