﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DAL;

namespace Backend.Migrations
{
    [DbContext(typeof(DataService))]
    [Migration("20170515035917_RefreshToken")]
    partial class RefreshToken
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DAL.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType")
                        .HasColumnName("fldContentType");

                    b.Property<byte[]>("File")
                        .HasColumnName("fldFile");

                    b.Property<int>("RecordId")
                        .HasColumnName("fldRecordId");

                    b.HasKey("Id");

                    b.ToTable("tbImage");
                });

            modelBuilder.Entity("DAL.Record", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnName("fldDateOfBirth");

                    b.Property<string>("FirstName")
                        .HasColumnName("fldFirstName");

                    b.Property<string>("Info")
                        .HasColumnName("fldInfo");

                    b.Property<string>("LastName")
                        .HasColumnName("fldLastName");

                    b.Property<string>("Phone")
                        .HasColumnName("fldPhone");

                    b.Property<string>("Surname")
                        .HasColumnName("fldSurname");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("tbRecord");
                });

            modelBuilder.Entity("DAL.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Token")
                        .HasColumnName("fldToken");

                    b.Property<int>("UserId")
                        .HasColumnName("fldUserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("tbRefreshToken");
                });

            modelBuilder.Entity("DAL.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .HasColumnName("fldEmail");

                    b.Property<byte[]>("Password")
                        .HasColumnName("fldPassword");

                    b.HasKey("Id");

                    b.ToTable("tbUser");
                });

            modelBuilder.Entity("DAL.Record", b =>
                {
                    b.HasOne("DAL.User", "User")
                        .WithMany("Records")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.RefreshToken", b =>
                {
                    b.HasOne("DAL.User", "User")
                        .WithOne("Token")
                        .HasForeignKey("DAL.RefreshToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
