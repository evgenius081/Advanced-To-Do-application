FROM node:18.16.0
RUN mkdir -p /app
WORKDIR /app
COPY . .

RUN npm install
RUN npm run build
ENV REACT_APP_ASP_LINK=$REACT_APP_ASP_LINK

HEALTHCHECK --interval=30s --timeout=30s --start-period=20s --retries=3 CMD [ "CMD", "curl", "-f", $REACT_APP_ASP_LINK ]

CMD [ "npm", "start" ]