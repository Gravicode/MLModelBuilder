﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ModelBuilder.Models
{
    public class MLModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public DateTime? Tanggal { get; set; }

        public string Nama { get; set; }
        public string Tipe { get; set; }
        public string Kolom { get; set; }
        public string TipeKolom { get; set; }
        public string DatasetPath { get; set; }
        public string ModelPath { get; set; }
        public string LabelName { get; set; }
        public string Deskripsi { get; set; }
        public string CreatedBy { get; set; }
        public string HasilTraining { get; set; }
    }
    public class UserProfile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Alamat { get; set; }
        public string? KTP { get; set; }
        public string? PicUrl { get; set; }
        public bool Aktif { get; set; } = true;

        [DataMember(Order = 11)]
        public Roles Role { set; get; } = Roles.User;

    }

    public enum Roles { Admin, User, Operator }
}