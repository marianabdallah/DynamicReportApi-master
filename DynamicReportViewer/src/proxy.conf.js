const PROXY_CONFIG = [
  {
    context: [
      "/SystemReport",
    ],
    target: "http://localhost:5136",
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
