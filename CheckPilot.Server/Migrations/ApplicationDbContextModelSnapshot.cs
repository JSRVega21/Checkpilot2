﻿// <auto-generated />
using System;
using CheckPilot.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CheckPilot.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CheckPilot.Models.InvoicePhoto", b =>
                {
                    b.Property<int>("InvoicePhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoicePhotoId"));

                    b.Property<byte[]>("BytePhoto")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("ByteSignature")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DocEntry")
                        .HasColumnType("int");

                    b.Property<int?>("DocNum")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumAtCard")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InvoicePhotoId");

                    b.ToTable("InvoicePhoto");
                });

            modelBuilder.Entity("CheckPilot.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserRole")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserRoleId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("UserCheckPilot");
                });

            modelBuilder.Entity("CheckPilot.Models.InvoicePhoto", b =>
                {
                    b.OwnsOne("CheckPilot.Models.RecordLog", "RecordLog", b1 =>
                        {
                            b1.Property<int>("InvoicePhotoId")
                                .HasColumnType("int");

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasComment("Usuario que creo el registro");

                            b1.Property<DateTime>("CreatedDate")
                                .HasColumnType("datetime2")
                                .HasComment("Fecha y hora de creación del registro");

                            b1.Property<bool>("IsActive")
                                .HasColumnType("bit")
                                .HasComment("Registro activo");

                            b1.Property<bool>("IsSystem")
                                .HasColumnType("bit")
                                .HasComment("Es un registro del sistema, los registros del sistema no pueden ser eliminados");

                            b1.Property<string>("ObjectKey")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)")
                                .HasComment("Código identificador del objeto representado en el registro");

                            b1.Property<string>("RecordKey")
                                .IsRequired()
                                .HasMaxLength(36)
                                .HasColumnType("nvarchar(36)")
                                .HasComment("Identificador único del registro, asignado en el momento de creación");

                            b1.Property<DateTime>("SyncDate")
                                .HasColumnType("datetime2")
                                .HasComment("Ultima fecha de sincronización");

                            b1.Property<int>("SyncStatus")
                                .HasColumnType("int")
                                .HasComment("Estatus de sincronización del registro");

                            b1.Property<string>("UpdatedBy")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasComment("Ultimo usuario que modificó el registro");

                            b1.Property<DateTime>("UpdatedDate")
                                .HasColumnType("datetime2")
                                .HasComment("Ultima fecha y hora de actualización del registro");

                            b1.HasKey("InvoicePhotoId");

                            b1.ToTable("InvoicePhoto");

                            b1.WithOwner()
                                .HasForeignKey("InvoicePhotoId");
                        });

                    b.Navigation("RecordLog");
                });

            modelBuilder.Entity("CheckPilot.Models.User", b =>
                {
                    b.OwnsOne("CheckPilot.Models.RecordLog", "RecordLog", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasComment("Usuario que creo el registro");

                            b1.Property<DateTime>("CreatedDate")
                                .HasColumnType("datetime2")
                                .HasComment("Fecha y hora de creación del registro");

                            b1.Property<bool>("IsActive")
                                .HasColumnType("bit")
                                .HasComment("Registro activo");

                            b1.Property<bool>("IsSystem")
                                .HasColumnType("bit")
                                .HasComment("Es un registro del sistema, los registros del sistema no pueden ser eliminados");

                            b1.Property<string>("ObjectKey")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)")
                                .HasComment("Código identificador del objeto representado en el registro");

                            b1.Property<string>("RecordKey")
                                .IsRequired()
                                .HasMaxLength(36)
                                .HasColumnType("nvarchar(36)")
                                .HasComment("Identificador único del registro, asignado en el momento de creación");

                            b1.Property<DateTime>("SyncDate")
                                .HasColumnType("datetime2")
                                .HasComment("Ultima fecha de sincronización");

                            b1.Property<int>("SyncStatus")
                                .HasColumnType("int")
                                .HasComment("Estatus de sincronización del registro");

                            b1.Property<string>("UpdatedBy")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasComment("Ultimo usuario que modificó el registro");

                            b1.Property<DateTime>("UpdatedDate")
                                .HasColumnType("datetime2")
                                .HasComment("Ultima fecha y hora de actualización del registro");

                            b1.HasKey("UserId");

                            b1.ToTable("UserCheckPilot");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("RecordLog");
                });
#pragma warning restore 612, 618
        }
    }
}
