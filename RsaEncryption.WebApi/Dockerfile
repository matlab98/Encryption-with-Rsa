FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /
COPY /zip/ .
EXPOSE 7119
ENV ASPNETCORE_URLS=http://*:7119
RUN ln -fs /usr/share/zoneinfo/America/Bogota /etc/localtime
RUN dpkg-reconfigure -f noninteractive tzdata
ENTRYPOINT ["dotnet", "RsaEncryption.WebApi.dll"]


