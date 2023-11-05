# base image
FROM nginx:1.16.0-alpine
# copy artifact build from the 'build environment'
COPY ./app/ /usr/share/nginx/html/
RUN rm -f /etc/nginx/conf.d/nginx.config
COPY ./nginx.config /etc/nginx/conf.d/nginx.config
# expose port 80
EXPOSE 80
# run nginx
CMD ["nginx", "-g", "daemon off;"]