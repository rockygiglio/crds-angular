FROM httpd
COPY ./httpd.conf  /usr/local/apache2/conf/httpd.conf
RUN chown :www-data /usr/local/apache2/conf/httpd.conf
ENV APACHE_LOG_DIR /tmp
EXPOSE 80