﻿// <auto-generated />
using System;
using EMedicalRecordAPISystemTask.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EMedicalRecordAPISystemTask.Migrations
{
    [DbContext(typeof(EMedicalRecordDbContext))]
    [Migration("20230425104204_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ClinicUser", b =>
                {
                    b.Property<int>("ClinicsID")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("ClinicsID", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ClinicUser");
                });

            modelBuilder.Entity("EMedicalRecordAPISystemTask.Models.AddressInfo", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("AppartmentNum")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HouseNum")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AddressInfo");
                });

            modelBuilder.Entity("EMedicalRecordAPISystemTask.Models.Clinic", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Clinics");
                });

            modelBuilder.Entity("EMedicalRecordAPISystemTask.Models.HumanInfo", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PersonalCode")
                        .HasColumnType("float");

                    b.Property<double>("PhoneNum")
                        .HasColumnType("float");

                    b.Property<byte[]>("ProfilePic")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("eMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HumanInfos");
                });

            modelBuilder.Entity("EMedicalRecordAPISystemTask.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("HumanInformationId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ClinicUser", b =>
                {
                    b.HasOne("EMedicalRecordAPISystemTask.Models.Clinic", null)
                        .WithMany()
                        .HasForeignKey("ClinicsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EMedicalRecordAPISystemTask.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EMedicalRecordAPISystemTask.Models.AddressInfo", b =>
                {
                    b.HasOne("EMedicalRecordAPISystemTask.Models.HumanInfo", "HumanInfo")
                        .WithOne("AddressInfo")
                        .HasForeignKey("EMedicalRecordAPISystemTask.Models.AddressInfo", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HumanInfo");
                });

            modelBuilder.Entity("EMedicalRecordAPISystemTask.Models.HumanInfo", b =>
                {
                    b.HasOne("EMedicalRecordAPISystemTask.Models.User", "User")
                        .WithOne("HumanInfo")
                        .HasForeignKey("EMedicalRecordAPISystemTask.Models.HumanInfo", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EMedicalRecordAPISystemTask.Models.HumanInfo", b =>
                {
                    b.Navigation("AddressInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("EMedicalRecordAPISystemTask.Models.User", b =>
                {
                    b.Navigation("HumanInfo")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}