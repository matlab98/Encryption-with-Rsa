FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY /zip/ .
EXPOSE 7119
ENV ASPNETCORE_URLS=http://0.0.0.0:7119
RUN ln -fs /usr/share/zoneinfo/America/Bogota /etc/localtime
RUN dpkg-reconfigure -f noninteractive tzdata
ENTRYPOINT ["dotnet", "RsaEncryption.WebApi.dll"]


