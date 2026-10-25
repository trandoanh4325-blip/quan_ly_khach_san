-- =====================================================================
-- ĐỒ ÁN C# - QUẢN LÝ KHÁCH SẠN
-- Script CSDL (MySQL / MariaDB trong XAMPP) - 19 bảng + dữ liệu mẫu
-- Cách chạy: phpMyAdmin -> tab Import -> chọn file này -> Go
-- =====================================================================
CREATE DATABASE IF NOT EXISTS qlks CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE qlks;
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS chi_tiet_phu_thu, chi_tiet_su_dung_dich_vu, hoa_don, chi_tiet_khach_o,
    chi_tiet_dat_phong, phieu_dat_phong, chi_tiet_phieu_nhap, phieu_nhap, nha_cung_cap,
    khuyen_mai, phu_thu, san_pham, loai_san_pham, dich_vu, phong, loai_phong,
    khach_hang, nhan_vien, tham_so;

SET FOREIGN_KEY_CHECKS = 1;

-- ---------------------------------------------------------------------
-- 1. Bảng tham số (mục 9 yêu cầu)
-- ---------------------------------------------------------------------
CREATE TABLE tham_so (
    ten_tham_so VARCHAR(50)  NOT NULL PRIMARY KEY,
    gia_tri     VARCHAR(255) NOT NULL,
    mo_ta       VARCHAR(255) NULL
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- 2. Nhân viên, Khách hàng
-- ---------------------------------------------------------------------
CREATE TABLE nhan_vien (
    ma_nv         VARCHAR(10)  NOT NULL PRIMARY KEY,
    ho_ten        VARCHAR(100) NOT NULL,
    chuc_vu       VARCHAR(50)  NOT NULL,
    so_dien_thoai VARCHAR(15)  NOT NULL UNIQUE
) ENGINE=InnoDB;

CREATE TABLE khach_hang (
    ma_kh        VARCHAR(10)  NOT NULL PRIMARY KEY,
    ho_ten       VARCHAR(100) NOT NULL,
    cccd         CHAR(12)     NOT NULL UNIQUE,
    gioi_tinh    ENUM('Nam','Nữ','Khác') NOT NULL,
    tong_thu     DECIMAL(15,0) NOT NULL DEFAULT 0,   -- tự cộng sau thanh toán (read-only)
    so_luot_toi  INT          NOT NULL DEFAULT 0     -- tự +1 sau thanh toán (read-only)
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- 3. Loại phòng, Phòng
-- ---------------------------------------------------------------------
CREATE TABLE loai_phong (
    ma_loai          VARCHAR(10)  NOT NULL PRIMARY KEY,
    ten_loai         VARCHAR(50)  NOT NULL,
    don_gia_ngay     DECIMAL(15,0) NOT NULL,
    don_gia_gio      DECIMAL(15,0) NOT NULL,
    so_luong_dang_co INT NOT NULL DEFAULT 0,          -- = số phòng đang trống
    tong_so_phong    INT NOT NULL DEFAULT 0
) ENGINE=InnoDB;

CREATE TABLE phong (
    ma_phong         VARCHAR(10) NOT NULL PRIMARY KEY,
    ten_phong        VARCHAR(50) NOT NULL,
    ma_loai          VARCHAR(10) NOT NULL,
    trang_thai       ENUM('Trống','Đã đủ') NOT NULL DEFAULT 'Trống',
    so_nguoi_toi_da  TINYINT NOT NULL,
    CONSTRAINT ck_phong_songuoi CHECK (so_nguoi_toi_da IN (2,4,6,8)),
    CONSTRAINT fk_phong_loai FOREIGN KEY (ma_loai) REFERENCES loai_phong(ma_loai)
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- 4. Dịch vụ
-- ---------------------------------------------------------------------
CREATE TABLE dich_vu (
    ma_dv        VARCHAR(10)  NOT NULL PRIMARY KEY,
    ten_dv       VARCHAR(100) NOT NULL,
    gia_tien     DECIMAL(15,0) NOT NULL,
    don_vi_tinh  VARCHAR(20)  NOT NULL
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- 5. Loại sản phẩm, Sản phẩm, Phụ thu
-- ---------------------------------------------------------------------
CREATE TABLE loai_san_pham (
    ma_loai_sp  VARCHAR(10)  NOT NULL PRIMARY KEY,
    ten_loai_sp VARCHAR(100) NOT NULL
) ENGINE=InnoDB;

CREATE TABLE san_pham (
    ma_sp        VARCHAR(10)  NOT NULL PRIMARY KEY,
    ten_sp       VARCHAR(100) NOT NULL,
    ma_loai_sp   VARCHAR(10)  NOT NULL,
    don_vi_tinh  VARCHAR(20)  NOT NULL,
    gia_ban      DECIMAL(15,0) NOT NULL,              -- giá bán / giá đền bù
    so_luong_ton INT NOT NULL DEFAULT 0,
    CONSTRAINT ck_sp_ton CHECK (so_luong_ton >= 0),
    CONSTRAINT fk_sp_loai FOREIGN KEY (ma_loai_sp) REFERENCES loai_san_pham(ma_loai_sp)
) ENGINE=InnoDB;

CREATE TABLE phu_thu (
    ma_phu_thu  VARCHAR(10)  NOT NULL PRIMARY KEY,
    ten_phu_thu VARCHAR(150) NOT NULL,
    ma_sp       VARCHAR(10)  NOT NULL,                -- bắt buộc, giá lấy từ sản phẩm
    CONSTRAINT fk_phuthu_sp FOREIGN KEY (ma_sp) REFERENCES san_pham(ma_sp)
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- 6. Khuyến mãi
-- ---------------------------------------------------------------------
CREATE TABLE khuyen_mai (
    ma_km               VARCHAR(10)  NOT NULL PRIMARY KEY,
    ten_km              VARCHAR(150) NOT NULL,
    loai_giam           ENUM('Phần trăm','Số tiền') NOT NULL,
    gia_tri_giam        DECIMAL(15,2) NOT NULL,       -- % hoặc số tiền gốc
    ngay_bat_dau        DATE NOT NULL,
    ngay_ket_thuc       DATE NOT NULL,
    tong_thu_toi_thieu  DECIMAL(15,0) NOT NULL DEFAULT 0,
    so_luot_toi_toi_thieu INT NOT NULL DEFAULT 0,
    CONSTRAINT ck_km_ngay CHECK (ngay_ket_thuc >= ngay_bat_dau)
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- 7. Nhập hàng
-- ---------------------------------------------------------------------
CREATE TABLE nha_cung_cap (
    ma_ncc        VARCHAR(10)  NOT NULL PRIMARY KEY,
    ten_ncc       VARCHAR(150) NOT NULL,
    so_dien_thoai VARCHAR(15)  NOT NULL,
    dia_chi       VARCHAR(255) NULL
) ENGINE=InnoDB;

CREATE TABLE phieu_nhap (
    ma_phieu_nhap  VARCHAR(10) NOT NULL PRIMARY KEY,
    ma_ncc         VARCHAR(10) NOT NULL,
    ma_nv          VARCHAR(10) NOT NULL,
    ngay_nhap      DATETIME    NOT NULL,
    tong_tien_nhap DECIMAL(15,0) NOT NULL DEFAULT 0,
    CONSTRAINT fk_pn_ncc FOREIGN KEY (ma_ncc) REFERENCES nha_cung_cap(ma_ncc),
    CONSTRAINT fk_pn_nv  FOREIGN KEY (ma_nv)  REFERENCES nhan_vien(ma_nv)
) ENGINE=InnoDB;

CREATE TABLE chi_tiet_phieu_nhap (
    ma_phieu_nhap  VARCHAR(10) NOT NULL,
    ma_sp          VARCHAR(10) NOT NULL,
    so_luong_nhap  INT NOT NULL,
    don_gia_nhap   DECIMAL(15,0) NOT NULL,
    thanh_tien     DECIMAL(15,0) NOT NULL,
    PRIMARY KEY (ma_phieu_nhap, ma_sp),
    CONSTRAINT fk_ctpn_pn FOREIGN KEY (ma_phieu_nhap) REFERENCES phieu_nhap(ma_phieu_nhap) ON DELETE CASCADE,
    CONSTRAINT fk_ctpn_sp FOREIGN KEY (ma_sp) REFERENCES san_pham(ma_sp)
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- 8. Đặt phòng
-- ---------------------------------------------------------------------
CREATE TABLE phieu_dat_phong (
    ma_phieu          VARCHAR(10) NOT NULL PRIMARY KEY,
    ma_kh             VARCHAR(10) NOT NULL,
    ma_nv             VARCHAR(10) NOT NULL,
    ngay_tao          DATETIME NOT NULL,
    ngay_checkin      DATETIME NOT NULL,
    ngay_checkout     DATETIME NULL,
    tien_coc          DECIMAL(15,0) NOT NULL DEFAULT 0,
    trang_thai_coc    ENUM('Chưa cọc','Đã cọc') NOT NULL DEFAULT 'Chưa cọc',
    so_ngay_o_du_kien INT NOT NULL DEFAULT 0,
    o_theo_gio        TINYINT NOT NULL DEFAULT 2,      -- 1 = Có, 2 = Không
    so_gio_nghi       INT NOT NULL DEFAULT 0,          -- chỉ nhập khi o_theo_gio = 1
    tong_tien_du_kien DECIMAL(15,0) NOT NULL DEFAULT 0,
    CONSTRAINT ck_pdp_theogio CHECK (o_theo_gio IN (1,2)),
    CONSTRAINT fk_pdp_kh FOREIGN KEY (ma_kh) REFERENCES khach_hang(ma_kh),
    CONSTRAINT fk_pdp_nv FOREIGN KEY (ma_nv) REFERENCES nhan_vien(ma_nv)
) ENGINE=InnoDB;

CREATE TABLE chi_tiet_dat_phong (
    ma_phieu        VARCHAR(10) NOT NULL,
    ma_phong        VARCHAR(10) NOT NULL,
    so_khach        INT NOT NULL DEFAULT 1,
    gia_phong       DECIMAL(15,0) NOT NULL,            -- đơn giá áp dụng (ngày hoặc giờ)
    ngay_gio_checkout DATETIME NULL,
    trang_thai      ENUM('Đang ở','Đã checkout') NOT NULL DEFAULT 'Đang ở',
    so_ngay_o       INT NOT NULL DEFAULT 0,
    so_gio_o        INT NOT NULL DEFAULT 0,
    thanh_tien      DECIMAL(15,0) NOT NULL DEFAULT 0,
    PRIMARY KEY (ma_phieu, ma_phong),
    CONSTRAINT fk_ctdp_phieu FOREIGN KEY (ma_phieu) REFERENCES phieu_dat_phong(ma_phieu) ON DELETE CASCADE,
    CONSTRAINT fk_ctdp_phong FOREIGN KEY (ma_phong) REFERENCES phong(ma_phong)
) ENGINE=InnoDB;

CREATE TABLE chi_tiet_khach_o (
    ma_khach_o  VARCHAR(12) NOT NULL PRIMARY KEY,      -- tự sinh, khác mã khách hàng
    ma_phieu    VARCHAR(10) NOT NULL,                  -- tự điền
    ma_phong    VARCHAR(10) NOT NULL,
    ho_ten      VARCHAR(100) NOT NULL,
    cccd        CHAR(12) NOT NULL,
    UNIQUE KEY uq_khacho_phieu_cccd (ma_phieu, cccd),  -- CCCD không trùng trong cùng phiếu
    CONSTRAINT fk_ctko_ctdp FOREIGN KEY (ma_phieu, ma_phong)
        REFERENCES chi_tiet_dat_phong(ma_phieu, ma_phong) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------
-- 9. Hóa đơn
-- ---------------------------------------------------------------------
CREATE TABLE hoa_don (
    ma_hoa_don         VARCHAR(10) NOT NULL PRIMARY KEY,
    ma_phieu           VARCHAR(10) NOT NULL UNIQUE,    -- mỗi phiếu chỉ có 1 hóa đơn
    ma_kh              VARCHAR(10) NOT NULL,
    ma_nv              VARCHAR(10) NOT NULL,
    ngay_thanh_toan    DATETIME NOT NULL,
    ma_km              VARCHAR(10) NULL,
    tien_da_coc        DECIMAL(15,0) NOT NULL DEFAULT 0,
    tien_giam          DECIMAL(15,0) NOT NULL DEFAULT 0,
    tong_tien_phong    DECIMAL(15,0) NOT NULL DEFAULT 0,
    tong_tien_dich_vu  DECIMAL(15,0) NOT NULL DEFAULT 0,
    tong_tien_phu_thu  DECIMAL(15,0) NOT NULL DEFAULT 0,
    tong_tien_hoa_don  DECIMAL(15,0) NOT NULL DEFAULT 0, -- phòng + dịch vụ + phụ thu - giảm
    tien_can_thanh_toan DECIMAL(15,0) NOT NULL DEFAULT 0,-- tổng tiền hóa đơn - tiền đã cọc
    CONSTRAINT fk_hd_phieu FOREIGN KEY (ma_phieu) REFERENCES phieu_dat_phong(ma_phieu),
    CONSTRAINT fk_hd_kh    FOREIGN KEY (ma_kh)    REFERENCES khach_hang(ma_kh),
    CONSTRAINT fk_hd_nv    FOREIGN KEY (ma_nv)    REFERENCES nhan_vien(ma_nv),
    CONSTRAINT fk_hd_km    FOREIGN KEY (ma_km)    REFERENCES khuyen_mai(ma_km)
) ENGINE=InnoDB;

CREATE TABLE chi_tiet_su_dung_dich_vu (
    ma_hoa_don       VARCHAR(10) NOT NULL,
    ma_phong         VARCHAR(10) NOT NULL,
    ma_dv            VARCHAR(10) NOT NULL,
    so_luong_su_dung INT NOT NULL,
    don_gia_su_dung  DECIMAL(15,0) NOT NULL,
    thanh_tien       DECIMAL(15,0) NOT NULL,
    PRIMARY KEY (ma_hoa_don, ma_phong, ma_dv),
    CONSTRAINT fk_ctdv_hd    FOREIGN KEY (ma_hoa_don) REFERENCES hoa_don(ma_hoa_don) ON DELETE CASCADE,
    CONSTRAINT fk_ctdv_phong FOREIGN KEY (ma_phong)   REFERENCES phong(ma_phong),
    CONSTRAINT fk_ctdv_dv    FOREIGN KEY (ma_dv)      REFERENCES dich_vu(ma_dv)
) ENGINE=InnoDB;

CREATE TABLE chi_tiet_phu_thu (
    ma_hoa_don  VARCHAR(10) NOT NULL,
    ma_phong    VARCHAR(10) NOT NULL,
    ma_phu_thu  VARCHAR(10) NOT NULL,
    so_luong    INT NOT NULL,
    don_gia     DECIMAL(15,0) NOT NULL,
    thanh_tien  DECIMAL(15,0) NOT NULL,
    ghi_chu     VARCHAR(255) NULL,
    PRIMARY KEY (ma_hoa_don, ma_phong, ma_phu_thu),
    CONSTRAINT fk_ctpt_hd    FOREIGN KEY (ma_hoa_don) REFERENCES hoa_don(ma_hoa_don) ON DELETE CASCADE,
    CONSTRAINT fk_ctpt_phong FOREIGN KEY (ma_phong)   REFERENCES phong(ma_phong),
    CONSTRAINT fk_ctpt_pt    FOREIGN KEY (ma_phu_thu) REFERENCES phu_thu(ma_phu_thu)
) ENGINE=InnoDB;

-- =====================================================================
-- DỮ LIỆU MẪU
-- =====================================================================
INSERT INTO tham_so (ten_tham_so, gia_tri, mo_ta) VALUES
('gio_nhan_phong',      '14:00', 'Giờ nhận phòng chuẩn (HH:mm)'),
('gio_tra_phong',       '12:00', 'Giờ trả phòng chuẩn (HH:mm)'),
('ty_le_coc_toi_thieu', '30',    'Tỷ lệ cọc tối thiểu (% trên tổng tiền dự kiến)'),
('so_phut_le_mien',     '15',    'Số phút lẻ được miễn khi tính số giờ lúc checkout'),
('ton_kho_canh_bao',    '5',     'Cảnh báo sản phẩm khi số lượng tồn nhỏ hơn hoặc bằng giá trị này'),
('ten_khach_san',       'Khách sạn Bình Minh', 'Tên khách sạn (hiện trên màn hình chính và report)'),
('dia_chi_khach_san',   '123 Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh', 'Địa chỉ khách sạn'),
('sdt_khach_san',       '0281234567', 'Số điện thoại khách sạn');

INSERT INTO nhan_vien VALUES
('NV001','Nguyễn Văn An','Quản lý','0901000001'),
('NV002','Trần Thị Bình','Lễ tân','0901000002'),
('NV003','Lê Hoàng Cường','Thủ kho','0901000003'),
('NV004','Phạm Thu Dung','Kế toán','0901000004');

INSERT INTO khach_hang (ma_kh, ho_ten, cccd, gioi_tinh, tong_thu, so_luot_toi) VALUES
('KH001','Nguyễn Minh Khôi','079200000001','Nam',0,0),
('KH002','Trần Thanh Hà','079200000002','Nữ',0,0),
('KH003','Lê Quốc Huy','079200000003','Nam',0,0),
('KH004','Phạm Ngọc Lan','079200000004','Nữ',0,0),
('KH005','Võ Gia Bảo','079200000005','Nam',6500000,4);

INSERT INTO loai_phong VALUES
('LP001','Standard',400000,80000,3,3),
('LP002','Deluxe',650000,120000,2,2),
('LP003','Family',900000,160000,2,2),
('LP004','Suite',1500000,250000,1,1);

INSERT INTO phong (ma_phong, ten_phong, ma_loai, trang_thai, so_nguoi_toi_da) VALUES
('P101','Phòng 101','LP001','Trống',2),
('P102','Phòng 102','LP001','Trống',2),
('P103','Phòng 103','LP001','Trống',2),
('P201','Phòng 201','LP002','Trống',4),
('P202','Phòng 202','LP002','Trống',4),
('P301','Phòng 301','LP003','Trống',6),
('P302','Phòng 302','LP003','Trống',6),
('P401','Phòng 401','LP004','Trống',8);

INSERT INTO dich_vu VALUES
('DV001','Ăn sáng',80000,'Suất'),
('DV002','Ăn trưa',120000,'Suất'),
('DV003','Ăn tối',150000,'Suất'),
('DV004','Massage',300000,'Giờ'),
('DV005','Bar',100000,'Lượt'),
('DV006','Gym',50000,'Lượt'),
('DV007','Hồ bơi',60000,'Lượt'),
('DV008','Xông hơi',100000,'Lượt'),
('DV009','Giặt ủi',30000,'Kg');

INSERT INTO loai_san_pham VALUES
('LSP001','Đồ uống'),
('LSP002','Đồ ăn'),
('LSP003','Thiết bị điện'),
('LSP004','Đồ vải'),
('LSP005','Đồ dùng phòng/vệ sinh');

INSERT INTO san_pham VALUES
('SP001','Nước suối','LSP001','Chai',10000,100),
('SP002','Coca-Cola','LSP001','Lon',20000,80),
('SP003','Bia Tiger','LSP001','Lon',30000,60),
('SP004','Snack khoai tây','LSP002','Gói',25000,50),
('SP005','Bánh quy','LSP002','Hộp',35000,40),
('SP006','Tivi','LSP003','Cái',8000000,5),
('SP007','Tủ lạnh mini','LSP003','Cái',3500000,3),
('SP008','Ga giường','LSP004','Bộ',450000,30),
('SP009','Chăn','LSP004','Cái',600000,20),
('SP010','Gối','LSP004','Cái',200000,40),
('SP011','Ly thủy tinh','LSP005','Cái',50000,60),
('SP012','Bồn cầu','LSP005','Cái',2500000,4);

INSERT INTO phu_thu VALUES
('PT001','Dùng nước suối trong phòng','SP001'),
('PT002','Dùng Coca-Cola trong phòng','SP002'),
('PT003','Dùng bia Tiger trong phòng','SP003'),
('PT004','Dùng snack trong phòng','SP004'),
('PT005','Dùng bánh quy trong phòng','SP005'),
('PT006','Hư hỏng tivi','SP006'),
('PT007','Hư hỏng tủ lạnh','SP007'),
('PT008','Hư hỏng / thiếu ga giường','SP008'),
('PT009','Hư hỏng / thiếu chăn','SP009'),
('PT010','Hư hỏng / thiếu gối','SP010'),
('PT011','Vỡ ly','SP011'),
('PT012','Hư hỏng nhà vệ sinh (bồn cầu)','SP012');

INSERT INTO khuyen_mai VALUES
('KM001','Khách thân thiết giảm 5%','Phần trăm',5,'2026-10-01','2026-12-31',5000000,3),
('KM002','Cuối năm giảm 10%','Phần trăm',10,'2026-12-01','2026-12-31',10000000,5),
('KM003','Giảm 200.000đ cho khách quen','Số tiền',200000,'2026-10-01','2026-12-31',2000000,2);

INSERT INTO nha_cung_cap VALUES
('NCC001','Công ty TNHH Thực phẩm Sài Gòn','0281112233','12 Lê Lợi, Quận 1, TP. Hồ Chí Minh'),
('NCC002','Công ty Thiết bị Khách sạn Á Châu','0284445566','45 Nguyễn Trãi, Quận 5, TP. Hồ Chí Minh');
