const express = require('express');
const app = express();
const port = process.env.PORT || 3000;

app.get('/api/', (req, res) => {
  res.send('Backend funcionando backoffice');
});

app.listen(port, () => {
  console.log(`Servidor escuchando en puerto ${port}`);
});